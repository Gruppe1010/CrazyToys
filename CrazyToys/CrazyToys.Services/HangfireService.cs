using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Hangfire;
using Hangfire.Server;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IcecatDataService _icecatDataService;
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;


        public HangfireService(
            IHttpClientFactory httpClientFactory, 
            IcecatDataService icecatDataService, 
            ISearchService<SolrToy> solrToyService, 
            ToyDbService toyDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _icecatDataService = icecatDataService;
            _solrToyService = solrToyService;
            _toyDbService = toyDbService;
            _priceGroupDbService = priceGroupDbService;

        }


        public async Task GetProductsDataService(string url, PerformContext context)
        {
            await _icecatDataService.GetProductsFromIcecat(url);
            BackgroundJob.ContinueJobWith(context.BackgroundJob.Id, () => UpdateSolrDb());
        }

        public async Task UpdateSolrDb()
        {
            // hent alle Toys op fra db
            List<Toy> toys = await _toyDbService.GetAllWithRelations();

            toys.ForEach(toy => {
                _solrToyService.CreateOrUpdate(new SolrToy(toy));
            });
        }
        
        public void DeleteSolrDb()
        {
            _solrToyService.DeleteAll();
        }
    }
}
