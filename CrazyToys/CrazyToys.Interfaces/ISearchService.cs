
using CrazyToys.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        Task<bool> CreateOrUpdate(T document);
        void DeleteAll();

        Dictionary<string, int> GetBrandFacets();

        Dictionary<string, int> GetCategoryFacets();

        List<string> GetAgeGroupsFacets();
    }
}
