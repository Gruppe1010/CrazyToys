using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services.SalesDbServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class RecommendationService : IRecommendationService
    {

        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly OrderDbService _orderDbService;


        public RecommendationService(
            ISearchService<SolrToy> solrToyService,
            OrderDbService orderDbService)
        {
            _solrToyService = solrToyService;
            _orderDbService = orderDbService;
        }



        public async Task<List<string>> FindRelatedToyIds(string toyId)
        {
            List<string> relatedToyIds = new List<string>();

            List<Order> orders = await _orderDbService.GetRelatedOrders(toyId);

            if(orders.Count > 0)
            {
                // key == OrderedToyId, value == antal gange id'et indgår i en orderLine på orders
                Dictionary<string, int> relatedToysFrequency = new Dictionary<string, int>();

                foreach(Order order in orders)
                {
                    foreach (OrderLine orderLine in order.OrderLines)
                    {
                        string orderedToyId = orderLine.OrderedToyId;

                        if (relatedToysFrequency.ContainsKey(orderedToyId))
                        {
                            relatedToysFrequency[orderedToyId] = relatedToysFrequency[orderedToyId] + 1;
                        }
                        else
                        {
                            relatedToysFrequency.Add(orderedToyId, 1);
                        }
                    }
                }
                // fjern det toyId som vises på detaljesiden
                relatedToysFrequency.Remove(toyId);

                relatedToyIds = relatedToysFrequency.OrderByDescending(x => x.Value).Select(x => x.Key).ToList();
            }
            return relatedToyIds;
        }

        public List<ShopToyDTO> GetRelatedShopToyDTOs(List<string> relatedToyIds, int amountToGet)
        {

            List<ShopToyDTO> shopToyDTOs = new List<ShopToyDTO>();

            for (int i = 0; i < amountToGet && i < relatedToyIds.Count; i++)
            {
                SolrToy solrToy = _solrToyService.GetById(relatedToyIds[i]);

                if(solrToy != null)
                {
                    shopToyDTOs.Add(solrToy.ConvertToShopToyDTO());
                }
            }

            return shopToyDTOs;
        }

    }
}
