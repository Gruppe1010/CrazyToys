using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.ProductDbServices;
using Hangfire;
using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IcecatDataService _icecatDataService;
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;


        public HangfireService(
            IHttpClientFactory httpClientFactory, 
            IcecatDataService icecatDataService, 
            ISearchService<SolrToy> solrToyService, 
            ToyDbService toyDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _icecatDataService = icecatDataService;
            _solrToyService = solrToyService;
            _toyDbService = toyDbService;
            _priceGroupDbService = priceGroupDbService;

        }

        public async Task GetProductsDataService(string url, PerformContext context)
        {
            await _icecatDataService.GetProductsFromIcecat(url);
            BackgroundJob.ContinueJobWith(context.BackgroundJob.Id, () => UpdateSolrDb(url.Contains("daily")));
        }


        public async Task UpdateSolrDb(bool isDaily)
        {
            string dateString = DateTime.Now.ToShortDateString();

            // hent alle Toys op fra db
            List<Toy> toys = await _toyDbService.GetToysToUpdateWithRelations(dateString);

            toys.ForEach(toy => {

                SolrToy newSolrToy = new SolrToy(toy);

                // hvis det er daily, så ligger der allerede solrToys på solr, og vi skal derfor hente dem ned for at få fat i soldAmount, så det kan komme med i opdateringen
                if (isDaily)
                {
                    SolrToy solrToy = _solrToyService.GetById(toy.ID);
                    if (solrToy != null)
                    {
                        newSolrToy.SoldAmount = solrToy.SoldAmount;
                    }
                }

                _solrToyService.CreateOrUpdate(newSolrToy);
            });
        }


        public void CreateOrderConfirmationJob(OrderConfirmationDTO orderConfirmationDTO)
        {
            BackgroundJob.Enqueue(() => SendOrderConfirmation(orderConfirmationDTO));
        }


        public void SendOrderConfirmation(OrderConfirmationDTO orderConfirmationDTO)
        {
            string bodyText = "";
            double subTotal = 0;
            double totalPrice;
        
            foreach (ShoppingCartToyDTO toy in orderConfirmationDTO.ShoppingCartToyDTOs)
            {
                var subAmount = toy.CalculateTotalPrice();

                bodyText = bodyText + "<tr></tr>" +  "<tr>" + "<td>" + "<img width='90' height='90' src='" + toy.Image + "'>" + "</td>" + "<td>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + toy.Name + "&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + toy.Quantity + " stk.&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td align='right'>" + subAmount + " DKK" + "</td>" + "</tr>";
                subTotal = subTotal + subAmount;
            }

            string freightPrice = "39 DKK";
            if (subTotal > 499)
            {
                totalPrice = subTotal;
                freightPrice = "Gratis levering";
            } 
            else
            {
                totalPrice = subTotal + 39;
            }


            var msgMail = new MailMessage();

            msgMail.From = new MailAddress("gruppe1010@hotmail.com");
            msgMail.To.Add(new MailAddress(orderConfirmationDTO.Email));
            msgMail.Subject = $"Din bestilling er modtaget! ({orderConfirmationDTO.OrderNumber}) ";

            msgMail.Body =
                $"<h1> Hej {orderConfirmationDTO.FirstName}!</h1>" +
                $"<h2> Tak for din ordre!</h2>" +


                $"<h2>Oplysninger </h2>" +
                $"<h4> Ordrenummer: {orderConfirmationDTO.OrderNumber} </h4>" +
                $"<h4> Ordredato: {orderConfirmationDTO.Date}</h4>" +
                $"<h4> Faktureringsadresse:</h4>" +
                $"<p>{orderConfirmationDTO.FirstName} {orderConfirmationDTO.LastName}</p>" +
                $"<p>{orderConfirmationDTO.BillingAddress.StreetAddress}</p>" +
                $"<p>{orderConfirmationDTO.BillingAddress.City}</p>" +
                $"<p>{orderConfirmationDTO.BillingAddress.Country}</p>" +


                $"<h4> Leveringssadresse:</h4>" +
                $"<p>{orderConfirmationDTO.FirstName} {orderConfirmationDTO.LastName}</p>" +
                $"<p>{orderConfirmationDTO.ShippingAddress.StreetAddress}</p>" +
                $"<p>{orderConfirmationDTO.ShippingAddress.City}</p>" +
                $"<p>{orderConfirmationDTO.ShippingAddress.Country}</p>" +


                "<h2> Følgende varer vil blive sendt til din adresse hurtigst muligt </h2>" +
                "<table>" +
                    "<tr> <th align='left'> Produkt </th> <th> </th> <th> Antal </th> <th align='right'> Pris </th></tr>" + 
                    $"<tbody>{bodyText}</tbody>" +
                    $"<tr><td>&nbsp;</td></tr>" +
                    $"<tr><th align='left'> Subtotal </th> <th> </th> <th> </th> <th align='right'> {subTotal} DKK" + "</th></tr>" +
                    $"<tr><th align='left'> Fragt </th> <th> </th> <th> </th> <th align='right'>{freightPrice} </th></tr>" +
                     "<tr><td>&nbsp;</td></tr>" +
                    $"<tr><th align='left'> Total </th> <th> </th> <th> </th> <th align='right'>{ totalPrice} DKK" + "</th></tr>" +
                "</table>";

            msgMail.IsBodyHtml = true;

            SmtpClient server = new SmtpClient();
            server.Host = "smtp.office365.com";
            server.Port = 587;
            server.EnableSsl = true;
            //TODO Sikkerhedsbrud!
            server.Credentials = new NetworkCredential("gruppe1010@hotmail.com", "mGG9-!SN=2Gb2Q#");
            server.Send(msgMail);
        }

        public void DeleteSolrDb()
        {
            _solrToyService.DeleteAll();
        }
    }
}
