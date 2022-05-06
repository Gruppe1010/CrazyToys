using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    // I hver service som implementerer dette interface, sender den den type Enititet tilbage i stedet for Object
    public interface IEntityCRUD<T>
    {
        Task<List<T>> GetAll();

        Task<List<T>> GetAllWithRelations();

        Task<T> GetById(string id);

        Task<T> Update(T t);

        Task<T> Create(T t);

        Task<T> CreateOrUpdate(T t);

        Task DeleteRange(IList<T> tList);

        Task<T> Delete(string id);






    }
}
