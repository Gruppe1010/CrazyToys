using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<ShopToyDTO>> GetRelatedToys(string toyId, int wantedAmount);

        Task<List<ShopToyDTO>> GetMostPopularToys(Toy toy, int wantedAmount);
    }
}


