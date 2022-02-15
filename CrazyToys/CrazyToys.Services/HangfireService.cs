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
        private readonly IProductDataService _icecatDataService;
        private readonly IEntityCRUD<Brand> _brandDbService;


        public HangfireService(IHttpClientFactory httpClientFactory, IProductDataService icecatDataService, IEntityCRUD<Brand> brandDbService)
        {
            _httpClientFactory = httpClientFactory;
            _icecatDataService = icecatDataService;
            _brandDbService = brandDbService;
        }

        public void GetDaily()
        {
            throw new NotImplementedException();
        }

        public async void GetIndex()
        {
            string credentials = _icecatDataService.GetIcecatCredentials();

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                //"https://data.Icecat.biz/export/freexml/EN/files.index.xml") // index
                "https://data.Icecat.biz/export/freexml/EN/daily.index.xml") // daily
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
                        switch (reader.Name)//(reader.NodeType)
                        {
                            case "file": //XmlNodeType.Element: // Her ved vi at det er en file

                                if (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    string supplierId = reader.GetAttribute("Supplier_id");
                                    Console.WriteLine("supplierId: " + supplierId);


                                    Brand brand = await _brandDbService.GetById(supplierId);
                                    //Brand brand = Task.Run(async () => await _brandDbService.GetById(supplierId)).Result;
                                    if (brand != null)
                                    {
                                        string productId = reader.GetAttribute("Prod_ID");

                                        // TODO måske Task.Run .Result noget
                                        var test = Task.Run(async () => await _icecatDataService.GetSingleProduct(supplierId, productId)).Result;

                                        //await _icecatDataService.GetSingleProduct(supplierId, productId);

                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                
            }
        }
    }
}
