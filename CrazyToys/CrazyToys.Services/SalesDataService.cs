using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.SalesDbServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class SalesDataService : ISalesDataService
    {

        private readonly CustomerDbService _customerDbService;
        private readonly CountryDbService _countrybService;


        public SalesDataService(CustomerDbService customerDbService, CountryDbService countrybService)
        {
            _customerDbService = customerDbService;
            _countrybService = countrybService;
        }

        public async Task<Order> CreateSale(CheckoutUserModel model)
        {
            var order = new Order();

            // opret eller få fat i den eksisterende bruger
            var customer = await GetOrCreateCustomer(model.Email);

            // 
            UpdateCustomerDetails(model, customer);

            return order;
        }

        public async Task<Customer> GetOrCreateCustomer(string email)
        {
            Customer customerFromDb = await _customerDbService.GetByEmail(email);

            return customerFromDb != null ? customerFromDb : await _customerDbService.Create(new Customer(email));
        }

        public async Task UpdateCustomerDetails(CheckoutUserModel model, Customer customer)
        {

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;


            // hent Country
            Country countryFromDb = await _countrybService.GetByName(model.CountryName);

            if(countryFromDb == null)
            {
                // TODO lav en fejl hvis den ikke har country i db - så er der sket en fejl i formen eller noget for det er en hardcodet select 

            }

            customer.BillingAddress = new Address(new City(model.CityName, model.PostalCode), model.StreetAddress, countryFromDb);

            

            




        }


        /*

        public async Task<Address> GetOrCreateCustomer(string email)
        {
            Customer customerFromDb = await _customerDbService.GetByEmail(email);

            return customerFromDb != null ? customerFromDb : await _customerDbService.Create(new Customer(email));
        }
        */
    }
}
