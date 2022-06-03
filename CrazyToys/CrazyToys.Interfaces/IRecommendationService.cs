using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<ShopToyDTO>> GetRelatedToys(string toyId, int amountToGet);

        Task<List<ShopToyDTO>> GetMostPopularToys(List<Category> categories, int wantedAmount);
    }
}


