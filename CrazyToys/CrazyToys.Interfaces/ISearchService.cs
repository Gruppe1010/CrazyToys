
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISearchService<T>
    {
        Task<bool> CreateOrUpdate(T document);
        //bool Delete(T document);
    }
}
