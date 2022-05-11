using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services;
using CrazyToys.Services.ProductDbServices;
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
        private readonly SalesDataService _salesDataService;
        private readonly IHangfireService _hangfireService;


        public CheckoutSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor,
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services,
            AppCaches appCaches,
            IProfilingLogger profilingLogger,
            IPublishedUrlProvider publishedUrlProvider,
            ISessionService sessionService,
            SalesDataService salesDataService,
            IHangfireService hangfireService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _sessionService = sessionService;
            _salesDataService = salesDataService;
            _hangfireService = hangfireService;
        }


        [HttpPost]
        public async Task<IActionResult> Submit(CheckoutUserModel model)
        {

            string urlPath = Environment.GetEnvironmentVariable("UrlPath");

            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            Dictionary<string, int> cart = sessionUser.Cart;

            if(cart.Count == 0)
            {
                // TODO tag imod denne i html
                ViewData["ErrorMessage"] = "Der ligger ikke noget i din kurv";
                return CurrentUmbracoPage();
            }

            Order newOrder = await _salesDataService.CreateSale(model, cart);

            if (newOrder == null)
            {
                return Redirect($"{urlPath}/shopping-cart");
            }

            // lav liste med toys som skal vises i orderConfirmation
            List<ShoppingCartToyDTO> orderConfirmationToyList = await _salesDataService.ConvertOrderLinesToShoppingCartToyDTOs(newOrder.OrderLines);



            // Opret orderConfirmationJob
            OrderConfirmationDTO orderConfirmationDTO = newOrder.ConvertToOrderConfirmationDTO(orderConfirmationToyList);
            _hangfireService.CreateOrderConfirmationJob(model, orderConfirmationDTO);

            // slet cart på sessionUser
            sessionUser.Cart.Clear();
            _sessionService.Update(HttpContext, sessionUser);

            // redirect til ny side/returner view med ordrenummer

            return Redirect($"{urlPath}/order-confirmation?orderNumber={newOrder.OrderNumber}");
        }
    }
}