

using CrazyToys.Entities.Models.Entities;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IProductDataService
    {
        

        Task<Toy> getSingleProductAsync(string brandId, string productId);

        


    }
}
