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
            httpClient.Timeout = TimeSpan.FromMinutes(1000);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Dictionary<string, Brand> brandDict = await _icecatDataService.GetBrandDict();
                string dateString = DateTime.Now.ToString();

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
                        Console.WriteLine(supplierId);

                        if (brandDict.ContainsKey(supplierId))
                        {
                            string productId = reader.GetAttribute("Prod_ID");
                            string onMarket = reader.GetAttribute("On_Market");
                            string GTIN13 = null;

                            if (reader.ReadToDescendant("EAN_UPCS"))
                            {
                                bool GTIN13Found = false;
                                while (!GTIN13Found)
                                {
                                    reader.Read(); //this moves reader to next node which is text 

                                    if (reader.GetAttribute("Format").Equals("GTIN-13"))
                                    {
                                        GTIN13 = reader.GetAttribute("Value"); //this might give value than 
                                        GTIN13Found = true;
                                    }
                                }
                            }
                      
                            SimpleToy simpleToy = await _icecatDataService.CreateSimpleToyInDb(new SimpleToy(supplierId, productId, onMarket, GTIN13, dateString));
                            /*
                            if (!productId.Contains("E+25"))
                            {
                                // Læg ned i ny SimpleToy-tabel
                                SimpleToy simpleToy = await _icecatDataService.CreateSimpleToyInDb(new SimpleToy(supplierId, productId, onMarket));
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
                            */

                        }
                    }
                }
                CreateToysFromSimpleToys(url.Contains("daily"), dateString);
            }
        }

        /*
         * Denne metoder henter alle SimpleToys op, som lige er blevet lagt i db ud fra index eller daily
         * Derefter henter den den fulde produktinfo i json, laver et nyt toy-obj og tilføjet til db 
         * hvis det er daily laver den CreateOrUpdateToyInDb - hvis det er index findes der ikke nogen toy-obj i db, og derfor laver den bare create
         * 
         * **/
        public async void CreateToysFromSimpleToys(bool isDaily, string dateString)
        {

            if (isDaily)
            {
                HashSet<SimpleToy> simpleToys = _icecatDataService.GetAllSimpleToysByDate(dateString);

                foreach (SimpleToy simpleToy in simpleToys)
                {

                    if (!simpleToy.ProductId.Contains("E+25"))
                    {
                        Toy toy = await _icecatDataService.GetSingleProduct(simpleToy);
                        Toy addedToy = await _icecatDataService.CreateOrUpdateToyInDb(toy);
                    }

                }

            }
            else
            {
                HashSet<SimpleToy> simpleToys = _icecatDataService.GetAllSimpleToysAsHashSet();

                foreach (SimpleToy simpleToy in simpleToys)
                {
                    if (!simpleToy.ProductId.Contains("E+25"))
                    {
                        Toy toy = await _icecatDataService.GetSingleProduct(simpleToy);
                        Toy addedToy = await _icecatDataService.CreateToyInDb(toy);
                    }
                }
            }
        }

        public void HelloHangfireWorld()
        {
            Console.WriteLine("Hej med dig verden");
        }
    }
}
