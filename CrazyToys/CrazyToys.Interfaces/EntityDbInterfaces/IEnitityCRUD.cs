using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    // I hver service som implementerer dette interface, sender den den type Enititet tilbage i stedet for Object
    public interface IEnitityCRUD<T>
    {
        Task<ICollection<T>> GetAll();

        Task<T> GetById(string id);

        Task<T> UpdateById(string id);

        Task<T> Create(T t);

        //Task<Object> Delete(string id);






    }
}
