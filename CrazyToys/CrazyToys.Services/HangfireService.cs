using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;

using Hangfire;
using Hangfire.Server;
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
            BackgroundJob.ContinueJobWith(context.BackgroundJob.Id, () => UpdateSolrDb());
        }

        public async Task UpdateSolrDb()
        {
            // hent alle Toys op fra db
            List<Toy> toys = await _toyDbService.GetAllWithRelations();

            toys.ForEach(toy => {
                _solrToyService.CreateOrUpdate(new SolrToy(toy));
            });
        }

        public void CreateOrderConfirmationJob(CheckoutUserModel model, List<ShoppingCartToyDTO> list)
        {
            BackgroundJob.Enqueue(() => SendOrderConfirmation(model, list));
        }

        public void SendOrderConfirmation(CheckoutUserModel model, List<ShoppingCartToyDTO> list)
        {
            string bodyText = "";
            double totalPrice = 0; 
            foreach (ShoppingCartToyDTO toy in list)
            {
                var subAmount = toy.CalculateTotalPrice();

                bodyText = bodyText + "<tr>" + "<td>" + toy.Name + "&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td>" + "&nbsp;&nbsp;&nbsp;&nbsp;" + toy.Quantity + " stk.&nbsp;&nbsp;&nbsp;&nbsp;" + "</td>" + "<td align='right'>" + subAmount + " DKK" + "</td>" + "</tr>";
                totalPrice = totalPrice + subAmount;
            }

            var msgMail = new MailMessage();

            msgMail.From = new MailAddress("gruppe1010@hotmail.com");
            msgMail.To.Add(new MailAddress(model.Email));
            msgMail.Subject = "Ordrebekræftelse fra Crazy Toys";

            msgMail.Body = "<h1> Tak for din ordre. </h1>" +
                "<h2> Følgende varer vil blive sendt til din adresse hurtigst muligt </h2>" +
                "<table>" +
                    "<tr> <th align='left'> Navn </th> <th> Antal </th> <th align='right'> Pris </th></tr>" + 
                    "<tbody>" + bodyText + "</tbody>" +
                    "<tr><td>&nbsp;</td></tr>" + 
                    "<tr><th align='left'> Total </th> <th> </th> <th align='right'>" + totalPrice + " DKK" + "</th></tr>" +
                "</table>";

            msgMail.IsBodyHtml = true;

            SmtpClient server = new SmtpClient();
            server.Host = "smtp.office365.com";
            server.Port = 587;
            server.EnableSsl = true;
            server.Credentials = new NetworkCredential("gruppe1010@hotmail.com", "DAT20v1!");
            server.Send(msgMail);
        }

        public void DeleteSolrDb()
        {
            _solrToyService.DeleteAll();
        }
    }
}
