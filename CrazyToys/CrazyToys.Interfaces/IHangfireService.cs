using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IHangfireService
    {
        Task GetProductsFromIcecat(string url);

        Task CreateToysFromSimpleToys(bool isDaily, string dateString); // TODO SLET DENNE IGEN

        void HelloHangfireWorld();
    }
}
