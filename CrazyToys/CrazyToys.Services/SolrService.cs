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
                // TODO noget med fejl/manglende på required fileds

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
            s = s + values[0] + ":";

            for(int i = 1; i < values.Length; i++)
            {
                s = s + "\"" + values[i] + "\"" + " OR ";
            }

            s = s.Substring(0, s.Length - 4) + ")"; // vi sletter det sidste OR

            return s;
        }
        
        public async Task<Dictionary<int, List<ShopToyDTO>>> GetToysForSinglePage(
            string category, 
            string subCategory, 
            string brands, 
            string price, 
            string ageGroups, 
            string colours, // rød.blå.grøn
            string page, 
            string search)
        {

            page = page ?? "1";

            var dict = new Dictionary<int, List<ShopToyDTO>>();

            // laver hver param om til fx "(color:rød OR grøn) AND"
            string categoryParam = category != null ? CreateFilterParam(category) + "AND" : null;
            string subCategoryParam = subCategory != null ? CreateFilterParam(subCategory) + "AND" : null;
            string brandsParam = brands != null ? CreateFilterParam(brands) + "AND" : null;
            string priceParam = price != null ? CreateFilterParam(price) + "AND" : null;
            string ageGroupsParam = ageGroups != null ? CreateFilterParam(ageGroups) + "AND" : null;
            string coloursParam = colours != null ? CreateFilterParam(colours) + "AND" : null;

            int start = (Int32.Parse(page)) * 30 - 30; //det sted hvor den skal starte (fordi page 2 starter på 30: 2 * 30 == 60 --> 60 - 30 --> 30)
            string paging = $"&rows=30&start={start}";

            string urlParams = (categoryParam + subCategoryParam + brandsParam + priceParam + ageGroupsParam + coloursParam);

            // Hvis der er nogle urlParams så sletter vi den sidste AND via urlParams.Substring(0, urlParams.Length - 3)
            urlParams = !String.IsNullOrWhiteSpace(urlParams) 
                ? urlParams.Substring(0, urlParams.Length - 3)
                : "*:*";

            string url = "http://solr:8983/solr/toys/select?indent=true&q.op=OR&q=" + HttpUtility.UrlEncode(urlParams).Replace("+", "%20") + paging;
            

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



        //TODO Fjern det her nå vi får hentet fra SQL i stedet
        /*
        public List<string> GetPriceGroupFacet()
        {
            List<string> priceGroup = new List<string>();

            var facets = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetFieldQuery("priceGroup") }
                }
            });
            // For at få result fra FacetFieldQuery skal FacetFields[] kaldes
            foreach (var facet in facets.FacetFields["priceGroup"])
            {
                priceGroup.Add(facet.Key);
            }

            return priceGroup;
        }
        */

        /*
        // TODO slet hvis det er
        public List<SolrToy> GetAll()
        {
            var searchResults = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 10,
                StartOrCursor = StartOrCursor.Cursor.Start,
                OrderBy = new[] {
                    new SortOrder("id", Order.DESC)
                }
            });

            var pagedResults = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 100,
                StartOrCursor = searchResults.NextCursorMark,
                OrderBy = new[] {
                    new SortOrder("id", Order.DESC)
                }
            });

            List<SolrToy> solrToys = new List<SolrToy>();

            foreach (var document in searchResults)
            {
                object solrToyObj = (object) document;
                solrToys.Add((SolrToy) solrToyObj);
            }

            return solrToys;
        }
        */
    }
}
