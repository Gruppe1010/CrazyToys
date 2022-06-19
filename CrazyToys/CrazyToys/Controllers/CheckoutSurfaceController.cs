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
        private readonly SalesService _salesService;
        private readonly IPaymentService _paymentService;

        [HttpPost]
        public async Task<IActionResult> Submit(CheckoutUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            Dictionary<string, int> cart = sessionUser.Cart;

            if(cart.Count == 0)
            {
                ViewData["ErrorMessage"] = "Der ligger ikke noget i din kurv";
                return CurrentUmbracoPage();
            }

            Order newOrder = await _salesService.CreateSale(model, cart);

            if (newOrder == null)
            {
                string urlPath = Environment.GetEnvironmentVariable("UrlPath");
                return Redirect($"{urlPath}/shopping-cart");
            }

            int paymentId = await _paymentService.CreatePayment(newOrder.OrderNumber.ToString(), "dkk");

            if (paymentId != 0)
            {
                // tilføj paymentId til newOrder og gem
                await _salesService.AddPaymentIdToOrder(newOrder, paymentId);

                string paymentUrl = await _paymentService.CreatePaymentLink(newOrder.OrderNumber, paymentId, newOrder.CalculateTotalPrice());

                return Redirect(paymentUrl);
            }
            ViewData["ErrorMessage"] = "Der gik noget galt i betalingen";
            return CurrentUmbracoPage();
        }



        public CheckoutSurfaceController(
      IUmbracoContextAccessor umbracoContextAccessor,
      IUmbracoDatabaseFactory databaseFactory,
      ServiceContext services,
      AppCaches appCaches,
      IProfilingLogger profilingLogger,
      IPublishedUrlProvider publishedUrlProvider,
      ISessionService sessionService,
      SalesService salesDataService,
      IPaymentService paymentService)
      : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _sessionService = sessionService;
            _salesService = salesDataService;
            _paymentService = paymentService;

        }
    }
}