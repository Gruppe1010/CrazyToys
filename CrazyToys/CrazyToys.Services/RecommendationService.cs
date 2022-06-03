using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
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

        private readonly ISearchService<SolrToy> _solrService;
        private readonly OrderDbService _orderDbService;


        public RecommendationService(
            ISearchService<SolrToy> solrService,
            OrderDbService orderDbService)
        {
            _solrService = solrService;
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

        public async Task<List<ShopToyDTO>> GetRelatedToys(string toyId, int amountToGet)
        {

            List<string> relatedToyIds = await FindRelatedToyIds(toyId);

            List<ShopToyDTO> shopToyDTOs = new List<ShopToyDTO>();

            for (int i = 0; i < amountToGet && i < relatedToyIds.Count; i++)
            {
                SolrToy solrToy = _solrService.GetById(relatedToyIds[i]);

                if(solrToy != null)
                {
                    shopToyDTOs.Add(solrToy.ConvertToShopToyDTO());
                }
            }

            return shopToyDTOs;
        }


        public async Task<List<ShopToyDTO>> GetMostPopularToys(List<Category> categories, int wantedAmount)
        {
            string query = ConvertCategoriesToQuery(categories);

            // category%3A"Bamser"OR"Dukker"
            string url = $"http://solr:8983/solr/toys/select?q={query}&rows={wantedAmount}";

            dynamic content = await _solrService.GetContent(url);

            Dictionary<int, List<ShopToyDTO>> toyDict = _solrService.GetToysFromContent(content);

            return toyDict.ElementAt(0).Value;
        }

        public string ConvertCategoriesToQuery(List<Category> categories)
        {
            //category % 3A % 22{ category}% 22

            string s = "category%3A";

            foreach (var category in categories)
            {
                s = $"%22{category.Name}%22OR";

            }

            return s.Substring(0, s.Length - 2);
        }

    }
}
