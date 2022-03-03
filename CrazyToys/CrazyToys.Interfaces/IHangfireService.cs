using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IHangfireService
    {
        Task GetProductsFromIcecat(string url, PerformContext context);

        Task CreateToysFromSimpleToys(bool isDaily, string dateString);

        Task UpdateSolrDb();

    }
}
