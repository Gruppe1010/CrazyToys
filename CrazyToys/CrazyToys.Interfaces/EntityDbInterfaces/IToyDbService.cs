using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    public interface IToyDbService
    {

        bool HasColour(string toyId, int colourId);

    }
}
