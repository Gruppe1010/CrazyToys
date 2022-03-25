using Hangfire.Server;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IHangfireService
    {
        Task GetProductsDataService(string url, PerformContext context);

        Task UpdateSolrDb();

        void DeleteSolrDb();

    }
}
