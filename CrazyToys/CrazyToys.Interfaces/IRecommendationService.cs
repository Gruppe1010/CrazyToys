using CrazyToys.Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IRecommendationService
    {
        Task<List<string>> FindRelatedToyIds(string toyId);

        List<ShopToyDTO> GetRelatedShopToyDTOs(List<string> relatedToyIds, int amountToGet);
    }
}
