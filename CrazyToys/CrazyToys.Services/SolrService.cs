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





        public SolrService(ISolrOperations<T> solr)
        {
            _solr = (TSolrOperations)solr;
        }
        

        public async Task<bool> CreateOrUpdate(T document)
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

        public Dictionary<string, int> GetBrandFacets()
        {
            Dictionary<string, int> brandsDict = new Dictionary<string, int>();

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

        public Dictionary<string, int> GetCategoryFacets()
        {
            Dictionary<string, int> categoryDict = new Dictionary<string, int>();

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

        public List<string> GetAgeGroupsFacets()
        {
            List<string> ageGroups = new List<string>();

            var facets = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetFieldQuery("ageGroup") }
                }
            });
            // For at få result fra FacetFieldQuery skal FacetFields[] kaldes
            foreach (var facet in facets.FacetFields["ageGroup"])
            {
                ageGroups.Add(facet.Key);
            }
            return ageGroups;
        }

      
        public List<string> GetColourFacets()
        {
            List<string> colours = new List<string>();

            var facets = _solr.Query(SolrQuery.All, new QueryOptions
            {
                Rows = 0,
                Facet = new FacetParameters
                {
                    Queries = new[] { new SolrFacetFieldQuery("colours") }
                }
            });
            // For at få result fra FacetFieldQuery skal FacetFields[] kaldes
            foreach (var facet in facets.FacetFields["colours"])
            {
                colours.Add(facet.Key);
            }
            return colours;
        }


    }
}
