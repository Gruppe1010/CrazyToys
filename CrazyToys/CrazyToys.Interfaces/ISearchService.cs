
using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.SolrModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        bool CreateOrUpdate(T document);
        void DeleteAll();
        void Delete(T document);

        T GetById(string id);

        SortedDictionary<string, int> GetBrandFacet();

        SortedDictionary<string, int> GetCategoryFacet();

        SortedDictionary<string, int> GetSubCategoryFacet();

        string CreateSearchUrl(
            string category, 
            string subCategory,
            string brands, 
            string priceGroup,
            string ageGroups, 
            string colours,
            int page, 
            string search, 
            string sort);

        Task<dynamic> GetContent(string url);

        Dictionary<int, List<ShopToyDTO>> GetToysFromContent(dynamic content);

        Dictionary<string, Dictionary<string, int>> GetFacetFieldsFromContent(dynamic content);

    }
}
