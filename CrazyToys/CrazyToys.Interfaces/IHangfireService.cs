using CrazyToys.Entities.DTOs;
using Hangfire.Server;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IHangfireService
    {
        Task GetProductsDataService(string url, PerformContext context);

        Task UpdateSolrDb();

        void DeleteSolrDb();

        void CreateOrderConfirmationJob(CheckoutUserModel model, List<ShoppingCartToyDTO> list);

    }
}
