using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Hangfire;
using Hangfire.Server;
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
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly ISearchService<SolrPriceGroup> _solrPriceGroupService;

        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;


        


        public HangfireService(
            IHttpClientFactory httpClientFactory, 
            IcecatDataService icecatDataService, 
            ISearchService<SolrToy> solrToyService, 
            ISearchService<SolrPriceGroup> solrPriceGroupService, 
            ToyDbService toyDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _icecatDataService = icecatDataService;
            _solrToyService = solrToyService;
            _toyDbService = toyDbService;
            _solrPriceGroupService = solrPriceGroupService;
            _priceGroupDbService = priceGroupDbService;

        }

        /*
         * Henter enten index eller daily-fil fra icecat
         * Den tjekker alle produkter om brand-id passer på vores udvalgte brands
         *      hvis ja --> henter den værdier til at lave SimpleToy-obj, som lægges ned i db
         * Efter ALLE produkter i fil er løbet igennem og lagt i db, hentes de op og oprettes som Toy-obj
         * 
         * De lægges FØRST ned i SimpleToy-tabel for at undgå at processen af at hente alt fra filen afbrydes, hvis der sker en fejl i hentningen af et enkelt produkt
         * **/
        public async Task GetProductsFromIcecat(string url, PerformContext context)
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

                        if (brandDict.ContainsKey(supplierId))
                        {
                            //string productId = reader.GetAttribute("Prod_ID");
                            string onMarket = reader.GetAttribute("On_Market");
                            string icecatId = reader.GetAttribute("Product_ID");

                            SimpleToy simpleToy = url.Contains("daily")
                                ? await _icecatDataService.CreateOrUpdateSimpleToyInDb(new SimpleToy(supplierId, onMarket, icecatId, dateString))
                                : await _icecatDataService.CreateSimpleToyInDb(new SimpleToy(supplierId, onMarket, icecatId, dateString));
                        }
                    }
                }
                // nu har vi lagt alle toys fra enten index eller daily successfuldt - nu skal de hentes op og lægges ned som Toy-objs
                var createAllToysTask = CreateToysFromSimpleToys(url.Contains("daily"), dateString);
                createAllToysTask.Wait();

                BackgroundJob.ContinueJobWith(context.BackgroundJob.Id, () => UpdateSolrDb());
                /*
                context
                BackgroundJob.ContinueWith(() => UpdateSolrDb());
                */
            }
        }

        /*
         * Denne metode henter alle SimpleToys op, som lige er blevet lagt i db ud fra index eller daily
         * Derefter henter den den fulde produktinfo i json, laver et nyt toy-obj og tilføjer til db 
         * - hvis det er daily laver den CreateOrUpdate 
         * - hvis det er index findes der ikke nogen toy-obj i db, og derfor laver den bare Create()
         ***/
        public async Task CreateToysFromSimpleToys(bool isDaily, string dateString)
        {
            HashSet<SimpleToy> simpleToys = isDaily
                ? _icecatDataService.GetAllSimpleToysByDate(dateString)
                : _icecatDataService.GetAllSimpleToysAsHashSet();

            foreach (SimpleToy simpleToy in simpleToys)
            {
                //if (!simpleToy.ProductId.Contains("E+25"))
                Toy toy = await _icecatDataService.GetSingleProduct(simpleToy);
                if (toy != null)
                {
                    Toy addedToy = isDaily
                                  ? await _icecatDataService.CreateOrUpdateToyInDb(toy)
                                  : await _icecatDataService.CreateToyInDb(toy);
                }
            }
        }

        /*
         * 
         * 
         * 
         * **/
        public async Task UpdateSolrDb()
        {
            await UpdateSolrToys();
            await UpdateSolrPriceGroups();
        }

        public async Task UpdateSolrToys()
        {
            // hent alle Toys op fra db
            List<Toy> toys = await _toyDbService.GetAllWithRelations();

            // convert to SolrToy
            List<SolrToy> solrToys = new List<SolrToy>();

            toys.ForEach(toy => {
                solrToys.Add(new SolrToy(toy));
            });

            // for each update den i solr
            solrToys.ForEach(solrToy => _solrToyService.CreateOrUpdate(solrToy));
        }

        public async Task UpdateSolrPriceGroups()
        {
            // hent alle Toys op fra db
            List<PriceGroup> priceGroups = await _priceGroupDbService.GetAll();

            // convert to SolrToy
            List<SolrPriceGroup> solrPriceGroups = new List<SolrPriceGroup>();

            priceGroups.ForEach(priceGroup => {
                solrPriceGroups.Add(new SolrPriceGroup(priceGroup));
            });

            // for each update den i solr
            solrPriceGroups.ForEach(solrPriceGroup => _solrPriceGroupService.CreateOrUpdate(solrPriceGroup));
        }




        public void DeleteSolrDb()
        {
            _solrToyService.DeleteAll();
        }
    }
}
