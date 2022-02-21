using CrazyToys.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    public interface ISimpleToyDbService
    {
        HashSet<SimpleToy> GetAllAsHashSet();


    }
}
