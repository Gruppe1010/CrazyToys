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

        public async Task<List<Order>> GetAllWithRelations()
        {
            var orders = await _salesContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderLines)
                .Include(o => o.ShippingAddress).ThenInclude(s => s.City)
                .Include(o => o.ShippingAddress).ThenInclude(s => s.Country)
                .Include(o => o.Statuses).ThenInclude(s => s.StatusType)
                .ToListAsync();
            return orders;
          
        }

        public async Task<Order> GetById(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                var order = await _salesContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderLines)
                    .Include(o => o.ShippingAddress).ThenInclude(s => s.City)
                    .Include(o => o.ShippingAddress).ThenInclude(s => s.Country)
                    .Include(o => o.Statuses).ThenInclude(s => s.StatusType)
                    .FirstOrDefaultAsync(o => o.ID == id);
                return order;
            }
            return null;
        }

        public async Task<Order> GetByOrderNumber(int orderNumber)
        {
            if (orderNumber != 0)
            {
                var order = await _salesContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.OrderLines)
                    .Include(o => o.ShippingAddress).ThenInclude(s => s.City)
                    .Include(o => o.ShippingAddress).ThenInclude(s => s.Country)
                    .Include(o => o.Statuses).ThenInclude(s => s.StatusType)
                    .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
                return order;
            }
            return null;
        }


        public async Task<List<Order>> GetRelatedOrders(string orderedToyId)
        {
            if(orderedToyId != null)
            {
                var orders = await _salesContext.Orders
                    .Include(o => o.OrderLines)
                    .Where(o => o.OrderLines.Any(ol => ol.OrderedToyId == orderedToyId))
                    .ToListAsync();
                return orders;
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

        public async Task<Order> GetLatest()
        {
            return await _salesContext.Orders.OrderByDescending(x => x.OrderNumber).FirstOrDefaultAsync();
        }
    }
}
