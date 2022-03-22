﻿using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using CrazyToys.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class CheckoutSurfaceController : SurfaceController
    {
        private readonly ISessionService _sessionService;
        private readonly ToyDbService _toyDbService;
        private readonly ISearchService<SolrToy> _solrToyService;



        public CheckoutSurfaceController(
            IUmbracoContextAccessor umbracoContextAccessor, 
            IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, 
            AppCaches appCaches, 
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider, 
            ISessionService sessionService, 
            ToyDbService toyDbService,
            ISearchService<SolrToy> solrToyService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _sessionService = sessionService;
            _toyDbService = toyDbService;
            _solrToyService = solrToyService;
        }


        [HttpPost]
        public async Task<IActionResult> Submit(CheckoutUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            foreach (var item in sessionUser.Cart)
            {
                Toy toy = await _toyDbService.GetById(item.Key);
                if(toy.Stock >= item.Value)
                {
                    toy.Stock = toy.Stock - item.Value;
                    await _toyDbService.Update(toy);
                    if(toy.Stock == 0)
                    {
                        _solrToyService.Delete(new SolrToy(toy));
                    }
                }
                else
                {
                    toy.Stock = 0;
                    await _toyDbService.Update(toy);
                    _solrToyService.Delete(new SolrToy(toy));

                }
            }


            //TODO Lav en Order entity Og få hangfire til at kalde en metode der sender email til kunde med en ordre bekfræftelse

            var noget = model.Firstname;

            sessionUser.Cart.Clear();
            _sessionService.Update(HttpContext, sessionUser);
            

            return Redirect("https://localhost:44325/order-confirmation");
        }
    }
}