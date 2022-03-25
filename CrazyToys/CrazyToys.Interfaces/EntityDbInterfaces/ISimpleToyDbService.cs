using CrazyToys.Entities.Entities;
using System.Collections.Generic;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    public interface ISimpleToyDbService
    {
        HashSet<SimpleToy> GetAllAsHashSet();


    }
}
