using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.SalesDbServices
{
    public class CustomerDbService : IEntityCRUD<Customer>
    {
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

        public Task<Customer> GetByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Update(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
