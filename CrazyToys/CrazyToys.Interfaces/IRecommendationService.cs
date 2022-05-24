using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> FindRelatedToyIds(string toyId);

        List<ShopToyDTO> GetRelatedShopToyDTOs(List<string> relatedToyIds, int amountToGet);

        Task<List<ShopToyDTO>> GetMostPopularToys(List<Category> categories, int wantedAmount);

    }
}
