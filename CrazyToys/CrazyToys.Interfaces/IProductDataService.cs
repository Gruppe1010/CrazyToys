using CrazyToys.Web.Models.Entities;
using System;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IProductDataService
    {

        Task<Toy> getSingleProductAsync(string brandId, string productId);



    }
}
