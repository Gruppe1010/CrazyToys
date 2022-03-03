﻿using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrNet;
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
