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
    public class SalesService : ISalesService
    {
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly CustomerDbService _customerDbService;
        private readonly CountryDbService _countryDbService;
        private readonly ToyDbService _toyDbService;
        private readonly OrderDbService _orderDbService;
        private readonly StatusTypeDbService _statusTypeDbService;
        private readonly CityDbService _cityDbService;
        private readonly AddressDbService _addressDbService;



        public SalesService(
            ISearchService<SolrToy> solrToyService,
            CustomerDbService customerDbService, 
            CountryDbService countryDbService,
            ToyDbService toyDbService,
            OrderDbService orderDbService,
            StatusTypeDbService statusTypeDbService,
            CityDbService cityDbService,
            AddressDbService addressDbService)
        {
            _solrToyService = solrToyService;
            _customerDbService = customerDbService;
            _countryDbService = countryDbService;
            _toyDbService = toyDbService;
            _orderDbService = orderDbService;
            _statusTypeDbService = statusTypeDbService;
            _cityDbService = cityDbService;
            _addressDbService = addressDbService;
        }


        /**
         Der skal altid være noget i cart-obj 
         */
        public async Task<Order> CreateSale(CheckoutUserModel model, Dictionary<string, int> cart)
        {
            Order newOrder = new Order();

            // tjek først for valid postalCode
            City city = await _cityDbService.GetByPostalCode(model.PostalCode);

            if (city == null || !city.Name.Equals(model.CityName))
            {
                // TODO ret lige her at den godt må skrive 
                return null;
            }

            // opret eller få fat i den eksisterende kunde, og tilknyt den ordren
            newOrder.Customer = await GetOrCreateCustomer(model.Email);
           
            // Opdaterer navn og BillingAddress
            await UpdateCustomerDetails(model, newOrder.Customer, city);

            // tilføj til customers ordrelist
            List<OrderLine> orderLines = await ConvertCartToOrderLines(cart);

            if(orderLines.Count > 0)
            {
                newOrder.OrderLines = orderLines;

                // TODO denne skal i fremtiden ændres så den ikke bare bliver til billing altid, men kan tilføjes seperat ude i formen på siden
                newOrder.ShippingAddress = newOrder.Customer.BillingAddress;
                // gem ned og returner

                newOrder = await _orderDbService.Create(newOrder);
                if(newOrder.ID != null) // hvis den faktisk er blevet oprettet, så skal den tilføje "Created-status"
                {
                    // TODO her skal UpdateSoldAmountInSolrToys() kaldes

                    StatusType createdStatusType = await _statusTypeDbService.GetByName("Created");
                    newOrder.Statuses.Add(new Status(createdStatusType, DateTime.Now));
                    return await _orderDbService.Update(newOrder);
                }
            }
            return null;
        }

        public async Task<Customer> GetOrCreateCustomer(string email)
        {
            Customer customerFromDb = await _customerDbService.GetByEmail(email);

            return customerFromDb != null ? customerFromDb : await _customerDbService.Create(new Customer(email));
        }

        /**
         Ændrer navn og billingaddress til det nye
         */
        public async Task UpdateCustomerDetails(CheckoutUserModel model, Customer customer, City city)
        {
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;

            // hent Country
            Country countryFromDb = await _countryDbService.GetByName(model.CountryName);

            if(countryFromDb == null)
            {
                // TODO lav en fejl hvis den ikke har country i db - så er der sket en fejl i formen eller noget for det er en hardcodet select 
            }

            customer.BillingAddress = await GetOrCreateAddress(city, model.StreetAddress, countryFromDb);
        }

        public async Task<Address> GetOrCreateAddress(City city, string streetAddress, Country country)
        {
            Address address = await _addressDbService.GetByCityAndStreetAddress(city, streetAddress);

            return address != null ? address : new Address(city, streetAddress, country);
        }


        // 
        public async Task<List<OrderLine>> ConvertCartToOrderLines(Dictionary<string, int> cart)
        {
            List<OrderLine> orderLines = new List<OrderLine>();

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
     Den tager imod de orderLines der er på den nyoprettede order, 
        og laver en liste med ShoppingCartToyDTO'er som vi skal bruge til at vise hvilke toys kunden har bestil i ordrebekræftelsen
     */
        public async Task<List<ShoppingCartToyDTO>> ConvertOrderLinesToShoppingCartToyDTOs(IList<OrderLine> orderLines)
        {
            List<ShoppingCartToyDTO> orderConfirmationToyList = new List<ShoppingCartToyDTO>();

            foreach (OrderLine orderLine in orderLines)
            {
                Toy toy = await _toyDbService.GetById(orderLine.OrderedToyId);
                orderConfirmationToyList.Add(toy.ConvertToShoppingCartToyDTO(orderLine.Quantity));
            }

            return orderConfirmationToyList;
        }



        public void UpdateSoldAmountInSolrToys(IList<OrderLine> orderLines)
        {
            foreach (OrderLine orderLine in orderLines)
            {
                SolrToy solrToy = _solrToyService.GetById(orderLine.OrderedToyId);

                if (solrToy != null)
                {
                    solrToy.SoldAmount += orderLine.Quantity;
                    _solrToyService.CreateOrUpdate(solrToy);
                }
                else 
                {
                    // TODO throw en exception
                }
            }
        }



    }
}
