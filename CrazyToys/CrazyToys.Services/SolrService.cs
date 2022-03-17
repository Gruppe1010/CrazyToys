using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrNet;
using SolrNet.Commands.Parameters;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CrazyToys.Services
{
    public class SolrService<T, TSolrOperations> : ISearchService<T>
        where TSolrOperations : ISolrOperations<T>
    {
        private readonly TSolrOperations _solr;
        private readonly IHttpClientFactory _httpClientFactory;

        public SolrService(ISolrOperations<T> solr, IHttpClientFactory httpClientFactory)
        {
            _solr = (TSolrOperations)solr;
            _httpClientFactory = httpClientFactory;
        }

        // TODO overvej at lav ISearhcService og SolrService om så den ikke bruger solrNet men kun HttpClient og http-kald

        public bool CreateOrUpdate(T document)
        {
            try
            {
                // If the id already exists, the record is updated, otherwise added
                _solr.Add(document);
                _solr.Commit();
                return true;
            }
            catch (Exception ex)
            {
                // TODO noget med fejl/manglende på required fields

                //Log exception
                Console.WriteLine("Solr ex: " + ex);
                return false;
            }
        }
        
        public void DeleteAll()
        {
            _solr.Delete(new SolrHasValueQuery("id"));
            _solr.Commit();
        }

        public SortedDictionary<string, int> GetBrandFacet()
        {
            SortedDictionary<string, int> brandsDict = new SortedDictionary<string, int>();

            var facets = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetFieldQuery("brand") }
                }
            });
            // For at få result fra FacetFieldQuery skal FacetFields[] kaldes
            foreach (var facet in facets.FacetFields["brand"])
            {
                brandsDict.Add(facet.Key, facet.Value);
            }

            return brandsDict;
        }

        public SortedDictionary<string, int> GetCategoryFacet()
        {
            SortedDictionary<string, int> categoryDict = new SortedDictionary<string, int>();
            
            var facets = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetFieldQuery("categories") }
                }
            });
            // For at få result fra FacetFieldQuery skal FacetFields[] kaldes
            foreach (var facet in facets.FacetFields["categories"])
            {
                categoryDict.Add(char.ToUpper(facet.Key[0]) + facet.Key.Substring(1), facet.Value);
            }

            return categoryDict;
        }

        // _colour.rød.blå.grøn ---> (colour: rød OR blå OR grøn)
        public string CreateFilterParam(string param)
        {
            if(param == null)
            {
                return null;
            }
            string s = "(";

            string[] values = param.Split('.');

            // "(colour:"
            s = s + values[0] + ":(";

            for(int i = 1; i < values.Length; i++)
            {
                s = s + "\"" + values[i] + "\",";
            }

            s = s.Substring(0, s.Length - 1) + "))"; // vi sletter det sidste OR

            return s;
        }
        
        /*
         * Laver en get-request til solr for at få fat i toys ud fra de valgte filtreringer på shop-siden
         * **/
        public async Task<Dictionary<int, List<ShopToyDTO>>> GetToysForSinglePage(
            string category, string subCategory, 
            string brands, string priceGroup, 
            string ageGroups, string colours, // fx: rød.blå.grøn
            int page,string search, string sort // fx: price_asc
            )
        {
            // sort=price asc
            sort = sort != null ? "&sort=" + sort.Replace("_", "%20") : null;
            page = page == 0 ? 1 : page;

            var dict = new Dictionary<int, List<ShopToyDTO>>();

            // laver hver param om til fx "(color:rød OR grøn) AND"
            string categoryParam = category != null ? CreateFilterParam(category) + "AND" : null;
            string subCategoryParam = subCategory != null ? CreateFilterParam(subCategory) + "AND" : null;
            string brandsParam = brands != null ? CreateFilterParam(brands) + "AND" : null;
            string priceParam = priceGroup != null ? CreateFilterParam(priceGroup) + "AND" : null;
            string ageGroupsParam = ageGroups != null ? CreateFilterParam(ageGroups) + "AND" : null;
            string coloursParam = colours != null ? CreateFilterParam(colours) + "AND" : null;

            //det sted hvor den skal starte (fordi page 2 starter på 30: 2 * 30 == 60 --> 60 - 30 --> 30)
            int start = page * 30 - 30; 
            string paging = $"&rows=30&start={start}";

            string urlParams = categoryParam + subCategoryParam + brandsParam + priceParam + ageGroupsParam + coloursParam;

            // Hvis der er nogle urlParams så sletter vi den sidste AND via urlParams.Substring(0, urlParams.Length - 3)
            urlParams = !String.IsNullOrWhiteSpace(urlParams) 
                ? urlParams.Substring(0, urlParams.Length - 3)
                : "*:*";
            
            string url = "http://solr:8983/solr/toys/select?indent=true&q.op=OR&q=" + HttpUtility.UrlEncode(urlParams).Replace("+", "%20") + paging + sort;
            

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, url)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    }
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(1000);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                List<ShopToyDTO> shopToyDTOs = new List<ShopToyDTO>();

                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                dynamic content = JObject.Parse(jsonString);

                var response = content.response;

                int numFound = response.numFound;

                foreach (var toy in response.docs)
                {
                    string id = toy.id;
                    string name = toy.name[0];
                    int price1 = toy.price;
                    string imageUrl = toy.image[0];

                    shopToyDTOs.Add(new ShopToyDTO(id, name, price1, imageUrl));
                }
                dict.Add(numFound, shopToyDTOs);
            }
            else
            {
                dict.Add(0, null);
            }
            return dict;
        }
     
    }
}
