﻿using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShopDetailsController : RenderController
    {
        private readonly ToyDbService _toyDbService;
        private readonly ISessionService _sessionService;

        public ShopDetailsController(
            ILogger<RenderController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            ToyDbService toyDbService,
            ISessionService sessionService
            ) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
            _sessionService = sessionService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string id)
        {
            var toy = await _toyDbService.GetById(id);

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            HashSet<string> wishlistToys = sessionUser.Wishlist;

            var noget = sessionUser.Cart.ContainsKey(id) ? sessionUser.Cart[id] : 0;

            ViewBag.Current = "Legetøjs Detaljer";
            ViewData["Toy"] = toy;
            ViewData["WishlistToys"] = wishlistToys;
            ViewData["AmountInCart"] = noget;
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
