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
    public class OrderDbService : IEntityCRUD<Order>
    {

        private readonly SalesContext _salesContext;

        public OrderDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public async Task<Order> Create(Order order)
        {
            _salesContext.Orders.Add(order);
            await _salesContext.SaveChangesAsync();

            return order;
        }

        public Task<Order> CreateOrUpdate(Order order)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<Order> order)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetById(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var order = await _salesContext.Orders
                    .FirstOrDefaultAsync(o => o.ID == id);
                return order;
            }
            return null;
        }

        public async Task<Order> Update(Order order)
        {
            _salesContext.Update(order);
            _salesContext.Entry(order).Property(nameof(Order.OrderNumber)).IsModified = false;
            await _salesContext.SaveChangesAsync();

            return order;
        }

        public Task<Order> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
