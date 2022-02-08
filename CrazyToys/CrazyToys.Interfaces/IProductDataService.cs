

using CrazyToys.Entities.Models.Entities;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces
{
    public interface IProductDataService
    {
        

        Task<Toy> getSingleProduct(string brandId, string productId);

        


    }
}
