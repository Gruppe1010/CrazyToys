
using CrazyToys.Entities.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        bool CreateOrUpdate(T document);
        void DeleteAll();
        void Delete(T document);

        SortedDictionary<string, int> GetBrandFacet();

        SortedDictionary<string, int> GetCategoryFacet();

        SortedDictionary<string, int> GetSubCategoryFacet();

        Task<dynamic> GetContent(
            string category, 
            string subCategory,
            string brands, 
            string priceGroup,
            string ageGroups, 
            string colours,
            int page, 
            string search, 
            string sort);

        Dictionary<int, List<ShopToyDTO>> GetToysFromContent(dynamic content);

        Dictionary<string, Dictionary<string, int>> GetFacetFieldsFromContent(dynamic content);

    }
}
