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
    public class CustomerDbService : IEntityCRUD<Customer>
    {

        private readonly SalesContext _salesContext;

        public CustomerDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public Task<Customer> Create(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> CreateOrUpdate(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<Customer> customers)
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Customer>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Customer> GetByEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                var customer = await _salesContext.Customers
                    .FirstOrDefaultAsync(c => c.Email == email);
                return customer;
            }
            return null;
        }

        public Task<Customer> Update(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}
