﻿using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.OrderEntities;
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

        void CreateOrderConfirmationJob(CheckoutUserModel model, OrderConfirmationDTO orderConfirmationDTO);

    }
}
