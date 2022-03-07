
using CrazyToys.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        bool CreateOrUpdate(T document);
        void DeleteAll();

        Dictionary<string, int> GetBrandFacet();

        Dictionary<string, int> GetCategoryFacet();

        List<string> GetAgeGroupsFacet();

        List<string> GetColourFacet();

    }
}
