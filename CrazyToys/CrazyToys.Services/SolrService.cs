using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
        
        public List<T> GetToysForSinglePage(
            string category, 
            string subCategory, 
            string brands, 
            string price, 
            string ageGroups, 
            string colours, 
            int page, 
            string search)
        {
            List <Toy> toys = new List<Toy>();

            string url = $"https://live.icecat.biz/api/?UserName={username}&Language=dk&icecat_id={simpleToy.IcecatId}";



            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, url)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/xml" },
                    }
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(1000);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {





            }




                return toys;
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
