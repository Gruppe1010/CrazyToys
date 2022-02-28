using CrazyToys.Interfaces;
using SolrNet;
using SolrNet.Exceptions;
using System;

namespace CrazyToys.Services
{
    public class SolrService<T, TSolrOperations> : ISearchService<T>
        where TSolrOperations : ISolrOperations<T>
    {
        private readonly TSolrOperations _solr;
        public SolrService(ISearchService<T> solr)
        {
            _solr = (TSolrOperations)solr;
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
            catch (SolrNetException ex)
            {
                //Log exception
                Console.WriteLine("Solr ex: " + ex);
                return false;
            }
        }

        /*
        public bool Delete(T document)
        {
            try
            {
                //Can also delete by id                
                _solr.Delete(document);
                _solr.Commit();
                return true;
            }
            catch (SolrNetException ex)
            {
                //Log exception
                Console.WriteLine("Solr ex: " + ex);
                return false;
            }
        }
        */
    }
}
