using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolrNet;
using SolrNet.Exceptions;
using System;
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
        

        public async Task<bool> CreateOrUpdate(T document)
        {
            try
            {
                // If the id already exists, the record is updated, otherwise added   
                _solr.Add(document);
                _solr.Commit();
                return true;

                /*
                SolrToy solrToy = (SolrToy)Convert.ChangeType(document, typeof(SolrToy));
                string url = "http://127.0.0.1:8983/solr/test/update/json/docs?commit=true";

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                    }
                };

                httpRequestMessage.Content = JsonContent.Create(new { id = "testId", name = "Vores test toy" });

                JObject payLoad = new JObject(
                new JProperty("id", "noget1"),
                new JProperty("name", "xxxxxx"));

                var httpContent = new StringContent(payLoad.ToString(), Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // Pass the handler to httpclient(from you are calling api)
                HttpClient httpClient = new HttpClient(clientHandler);

                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromMinutes(1000);
                var httpResponseMessage = await httpClient.PostAsync(url, httpContent);
                string noget = "hej";

                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    return true;


                }
                */
            }
            catch (Exception ex)
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
