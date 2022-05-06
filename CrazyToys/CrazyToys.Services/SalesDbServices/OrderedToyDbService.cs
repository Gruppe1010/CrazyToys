using CrazyToys.Data.Data;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.SalesDbServices
{
    public class OrderedToyDbService : IEntityCRUD<OrderedToy>
    {

        private readonly SalesContext _salesContext;

        public OrderedToyDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public Task<OrderedToy> Create(OrderedToy orderedToy)
        {
            throw new NotImplementedException();
        }

        public Task<OrderedToy> CreateOrUpdate(OrderedToy orderedToy)
        {
            throw new NotImplementedException();
        }

        public Task<OrderedToy> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<OrderedToy> orderedToys)
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderedToy>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<OrderedToy>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<OrderedToy> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderedToy> GetByProductId(string productId)
        {
            if (!string.IsNullOrWhiteSpace(productId))
            {
                var orderedToy = await _salesContext.OrderedToys
                    .FirstOrDefaultAsync(o => o.ProductId == productId);
                return orderedToy;
            }
            return null;
        }

        public Task<OrderedToy> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<OrderedToy> Update(OrderedToy orderedToy)
        {
            throw new NotImplementedException();
        }
    }
}
