using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    internal class SalesDataService : ISalesDataService
    {
        public async Task<Order> CreateSale(CheckoutUserModel model)
        {
            var order = new Order();
            var customer = GetOrCreateCustomer(model.Email);
            UpdateCustomerDetails(customer, model);

            return order;
        }

        public Customer GetOrCreateCustomer(string email)
        {
            return new Customer();
        }

        public void UpdateCustomerDetails(Customer c, CheckoutUserModel model)
        {

        }
    }
}
