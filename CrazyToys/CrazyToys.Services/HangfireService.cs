using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CrazyToys.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IcecatDataService _icecatDataService;

        //private readonly IProductDataService _icecatDataService;
        private readonly IEntityCRUD<Brand> _brandDbService;



        public HangfireService(IHttpClientFactory httpClientFactory, IcecatDataService icecatDataService,
            IEntityCRUD<Brand> brandDbService)
        {
            _httpClientFactory = httpClientFactory;
            _icecatDataService = icecatDataService;
            _brandDbService = brandDbService;
        }

        public async Task GetProductsFromIcecat(string url)
        {
            string credentials = _icecatDataService.GetIcecatCredentials();

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, url)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/xml" },
                        { HeaderNames.Authorization, $"Basic {credentials} " }
                    }
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(10);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;
                settings.IgnoreWhitespace = true;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.IgnoreComments = true;

                XmlReader reader = XmlReader.Create(contentStream, settings);

                while (await reader.ReadAsync())
                {

                    if (reader.Name == "file" && (reader.NodeType != XmlNodeType.EndElement))
                    {
                        string supplierId = reader.GetAttribute("Supplier_id");

                        Brand brand = await _brandDbService.GetById(supplierId);
                        if (brand != null)
                        {
                            string productId = reader.GetAttribute("Prod_ID");
                            Console.WriteLine("productId" + productId);
                            string onMarket = reader.GetAttribute("On_Market");

                            Toy toy = await _icecatDataService.GetSingleProduct(supplierId, productId, onMarket);

                            if (url.Contains("daily"))
                            {
                                Toy addedToy = await _icecatDataService.CreateOrUpdateToyInDb(toy);
                            } 
                            else
                            {
                                Toy addedToy = await _icecatDataService.CreateToyInDb(toy);
                            }
                        }
                    }
                }
            }
        }
    }
}
