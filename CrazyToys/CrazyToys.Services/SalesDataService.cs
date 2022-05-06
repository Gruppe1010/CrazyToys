using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.ProductDbServices;
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
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly CustomerDbService _customerDbService;
        private readonly CountryDbService _countrybService;
        private readonly ToyDbService _toyDbService;
        private readonly OrderDbService _orderDbService;


        public SalesDataService(
            ISessionService sessionService,
            ISearchService<SolrToy> solrToyService,
            CustomerDbService customerDbService, 
            CountryDbService countrybService,
            ToyDbService toyDbService,
            OrderDbService orderDbService)
        {
            _solrToyService = solrToyService;
            _customerDbService = customerDbService;
            _countrybService = countrybService;
            _toyDbService = toyDbService;
            _orderDbService = orderDbService;
        }



        public async Task<Order> CreateSale(CheckoutUserModel model, Dictionary<string, int> cart)
        {
            Order newOrder = new Order();

            // opret eller få fat i den eksisterende kunde, og tilknyt den ordren
            newOrder.Customer = await GetOrCreateCustomer(model.Email);
           
            // Opdaterer navn og BillingAddress
            await UpdateCustomerDetails(model, newOrder.Customer);

            // tilføj til customers ordrelist
            newOrder.OrderLines = await ConvertCartToOrderLines(cart);

            // TODO sørg for at den returnerer denne eller noget, fordi den skal sendes med - kig på gammel kode - her på mandag - nu skal i afsted og vinde trofæet
            //orderConfirmationToyList.Add(toy.ConvertToShoppingCartToyDTO(item.Value));

            // gem ned
            return await _orderDbService.Create(newOrder);
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

        public async Task<List<OrderLine>> ConvertCartToOrderLines(Dictionary<string, int> cart)
        {
            List<OrderLine> orderLines = new List <OrderLine>();

            foreach (var item in cart)
            {
                // hent Toy op fra db
                Toy toy = await _toyDbService.GetById(item.Key);

                if (toy.Stock >= item.Value)
                {
                    toy.Stock = toy.Stock - item.Value;
                    await _toyDbService.Update(toy);
                    if (toy.Stock == 0)
                    {
                        _solrToyService.Delete(new SolrToy(toy));
                    }

                    // lav en ny OrderLine
                    OrderLine orderLine = new OrderLine(item.Key, item.Value, toy.Price, 0); // altid 0 i rabat, fordi vi ikke har rabatter
                    orderLines.Add(orderLine);
                }
                else // hvis der er færre på lager end der er i min kurv, så skal den tilføje det antal som er på lager - 
                {
                    // TODO lav noget sådan så man kan se at man kun har fået 5 og ikke de 7 man faktisk ville have
                    OrderLine orderLine = new OrderLine(item.Key, toy.Stock, toy.Price, 0); // altid 0 i rabat, fordi vi ikke har rabatter
                    orderLines.Add(orderLine);

                    toy.Stock = 0;
                    await _toyDbService.Update(toy);
                    _solrToyService.Delete(new SolrToy(toy));
                }

                //TODO toyStock skal IKKE ændres - den skal ændre i reservedAmount


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
