
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        bool CreateOrUpdate(T document);
        void DeleteAll();

        SortedDictionary<string, int> GetBrandFacet();

        SortedDictionary<string, int> GetCategoryFacet();

        //TODO Fjern det her nå vi får hentet fra SQL i stedet
        /*
        List<string> GetPriceGroupFacet();
        */

        /*
        // TODO slet hvis det er
        List<SolrToy> GetAll();
        */
    }
}
