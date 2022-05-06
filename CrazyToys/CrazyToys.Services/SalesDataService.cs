using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.SalesDbServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class SalesDataService : ISalesDataService
    {
        private readonly ISessionService _sessionService;
        private readonly CustomerDbService _customerDbService;
        private readonly CountryDbService _countrybService;
        private readonly OrderedToyDbService _orderedToyDbService;




        public SalesDataService(
            ISessionService sessionService,
            CustomerDbService customerDbService, 
            CountryDbService countrybService,
            OrderedToyDbService orderedToyDbService)
        {
            _customerDbService = customerDbService;
            _countrybService = countrybService;
            _orderedToyDbService = orderedToyDbService;
        }

        public async Task<Order> CreateSale(CheckoutUserModel model, Dictionary<string, int> cart)
        {
            Order order = new Order();

            // opret eller få fat i den eksisterende bruger
            Customer customer = await GetOrCreateCustomer(model.Email);

            // Opdaterer navn og BillingAddress
            await UpdateCustomerDetails(model, customer);

            // opret ny ordre ud fra de toys der er i sessionUserCart
            Order newOrder = new Order();
            newOrder.OrderLines = ConvertCartToOrderLines(cart);


            // tilføj til customers ordrelist

            // gem ned


            return order;
        }

        public async Task<Customer> GetOrCreateCustomer(string email)
        {
            Customer customerFromDb = await _customerDbService.GetByEmail(email);

            return customerFromDb != null ? customerFromDb : await _customerDbService.Create(new Customer(email));
        }


        /**
         Ændrer navn og billingaddress til det nye
         
         */
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

            // TODO test om den laver en fejl hvis man prøver med den præcis samme addresse to gange i streg
            customer.BillingAddress = new Address(new City(model.CityName, model.PostalCode), model.StreetAddress, countryFromDb);
        }

        public List<OrderLine> ConvertCartToOrderLines(Dictionary<string, int> cart)
        {
            List<OrderLine> orderLines = new List <OrderLine>();

            foreach (var item in cart)
            {

                // tjek om dette toy allerede ligger i tabllen 
                _orderedToyDbService


                /*
                Toy toy = await _toyDbService.GetById(item.Key);
                orderConfirmationToyList.Add(toy.ConvertToShoppingCartToyDTO(item.Value));

                if (toy.Stock >= item.Value)
                {
                    toy.Stock = toy.Stock - item.Value;
                    await _toyDbService.Update(toy);
                    if (toy.Stock == 0)
                    {
                        _solrToyService.Delete(new SolrToy(toy));
                    }
                }
                else
                {
                    //TODO Hvis 7 købes men kun 5 på lager. skriv 5 købt
                    toy.Stock = 0;
                    await _toyDbService.Update(toy);
                    _solrToyService.Delete(new SolrToy(toy));
                }
                */
            }

            return orderLines;

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
