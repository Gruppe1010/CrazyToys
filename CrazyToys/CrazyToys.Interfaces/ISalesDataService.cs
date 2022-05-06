using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using System;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISalesDataService
    {
        Task<Order> CreateSale(CheckoutUserModel model);

    }
}
