using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface ISalesService
    {
        Task<Order> CreateSale(CheckoutUserModel model, Dictionary<string, int> cart);

    }
}
