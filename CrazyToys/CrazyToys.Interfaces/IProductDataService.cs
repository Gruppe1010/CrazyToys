﻿

using CrazyToys.Entities.Entities;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IProductDataService
    {
        string GetIcecatCredentials();

        Task<Toy> GetSingleProduct(string brandId, string productId);

        


    }
}
