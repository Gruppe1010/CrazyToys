using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class CheckoutSurfaceController : SurfaceController
    {
        private readonly ISessionService _sessionService;
        private readonly ToyDbService _toyDbService;
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly IHangfireService _hangfireService;

        public CheckoutSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            ISessionService sessionService,
            ToyDbService toyDbService,
            ISearchService<SolrToy> solrToyService,
            IHangfireService hangfireService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _sessionService = sessionService;
            _toyDbService = toyDbService;
            _solrToyService = solrToyService;
            _hangfireService = hangfireService;
        }


        [HttpPost]
        public async Task<IActionResult> Submit(CheckoutUserModel model)
        {
            string UrlPath = Environment.GetEnvironmentVariable("UrlPath");

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }
            
            var orderConfirmationToyList = new List<ShoppingCartToyDTO>();
            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            foreach (var item in sessionUser.Cart)
            {
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
            }

            //Laver job i hangfire og kalder metode der sender OrderConfirmation til kunden
            _hangfireService.CreateOrderConfirmationJob(model, orderConfirmationToyList);

            sessionUser.Cart.Clear();
            _sessionService.Update(HttpContext, sessionUser);

            return Redirect($"{UrlPath}/order-confirmation");
        }
    }
}