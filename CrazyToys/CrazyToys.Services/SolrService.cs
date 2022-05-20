using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using Microsoft.Net.Http.Headers;
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

        public T GetById(string id)
        {
            var toy = _solr.Query(new SolrQueryByField("id", id));

            return toy.Count > 0 ? toy[0] : default(T);
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
            if (param == null)
            {
                return null;
            }
            string s = "fq={!tag=";

            string[] values = param.Split('.');

            // filterOption == navnet på en af propertiesne på et solrToy - fx colourGroups
            string filterOption = values[0];

            // "fq={!tag=colourGroups
            s = s + filterOption + "}" + filterOption + ":(";

            for (int i = 1; i < values.Length; i++)
            {
                s = s + "\"" + values[i] + "\",";
            }

            s = s.Substring(0, s.Length - 1) + ")"; // vi sletter det sidste komma

            return s;
        }

        

        public void Delete(T document)
        {
            _solr.Delete(document);
            _solr.Commit();
        }

        public SortedDictionary<string, int> GetSubCategoryFacet()
        {
            throw new NotImplementedException();
        }

        public async Task<dynamic> GetContent(string category, string subCategory,
            string brands, string priceGroup,
            string ageGroups, string colours, // fx: rød.blå.grøn
            int page, string search, string sort)
        {
            string mainQuery = "";
            if (!String.IsNullOrWhiteSpace(search))
            {
                string searchWordsInString = "(";

                string[] searchWords = search.Split(' ');

                foreach(string searchWord in searchWords)
                {
                    searchWordsInString = searchWordsInString + $"\"{searchWord}\",";
                }
                searchWordsInString = searchWordsInString.Substring(0, searchWordsInString.Length - 1);

                searchWordsInString = searchWordsInString + ")";
                mainQuery = $"q={searchWordsInString}";
            }
           

            // sort=price_desc
            sort = sort != null ? "&sort=" + sort.Replace("_", "%20") : null;
            //det sted hvor den skal starte (fordi page 2 starter på 30: 2 * 30 == 60 --> 60 - 30 --> 30)
            // "" hvis page 0 fordi så bruger den default-start 0
            string paging = page == 0 ? "" : $"&start={page * 30 - 30}";

            // laver hver param om til fx "(color:rød OR grøn) AND"
            string categoryParam = category != null ? CreateFilterParam(category) + "&" : null;
            string subCategoryParam = subCategory != null ? CreateFilterParam(subCategory) + "&" : null;
            string brandsParam = brands != null ? CreateFilterParam(brands) + "&" : null;
            string priceParam = priceGroup != null ? CreateFilterParam(priceGroup) + "&" : null;
            string ageGroupsParam = ageGroups != null ? CreateFilterParam(ageGroups) + "&" : null;
            string coloursParam = colours != null ? CreateFilterParam(colours) + "&" : null;

            string urlParams = categoryParam + subCategoryParam + brandsParam + priceParam + ageGroupsParam + coloursParam;

            urlParams = !String.IsNullOrWhiteSpace(urlParams)
                ? urlParams.Substring(0, urlParams.Length - 1)
                : "";

            string url = "http://solr:8983/solr/toys/select?" + mainQuery + paging + sort + "&" + urlParams.Replace("+", "%20"); 

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
                var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
                dynamic content = JObject.Parse(jsonString);
                return content;
            }

            return null;
        }

        public Dictionary<int, List<ShopToyDTO>> GetToysFromContent(dynamic content)
        {
            var toyDict = new Dictionary<int, List<ShopToyDTO>>();

            if(content != null)
            {
                List<ShopToyDTO> shopToyDTOs = new List<ShopToyDTO>();

                var response = content.response;

                int numFound = response.numFound;

                foreach (var toy in response.docs)
                {
                    string id = toy.id;
                    string name = toy.name[0];
                    int price = toy.price;
                    string imageUrl = toy.image;

                    shopToyDTOs.Add(new ShopToyDTO(id, name, price, imageUrl));
                }
                toyDict.Add(numFound, shopToyDTOs);
            }
            else
            {
                toyDict.Add(0, null);
            }
         
            return toyDict;
        }

        public Dictionary<string, Dictionary<string, int>> GetFacetFieldsFromContent(dynamic content)
        {
            var facetFieldDict = new Dictionary<string, Dictionary<string, int>>();

            if (content != null)
            {
                foreach (var facet in content.facet_counts.facet_fields)
                {
                    Dictionary<string, int> facetValueDict = new Dictionary<string, int>();

                    var key = facet.Name;
                    var facetValues = facet.Value;

                    facetFieldDict.Add(key, facetValueDict);

                    string facetValueName = null;
                    int facetValueQuantity;

                    for (int i = 0; i < facetValues.Count; i++)
                    {
                        if (i % 2 == 1)
                        {
                            facetValueQuantity = facetValues[i];
                            facetFieldDict[key].Add(facetValueName, facetValueQuantity);
                        } 
                        else
                        {
                            facetValueName = facetValues[i];
                        }
                    }
                }

            }
            else
            {
                facetFieldDict.Add(null, null);
            }

            return facetFieldDict;
        }
    }
}
