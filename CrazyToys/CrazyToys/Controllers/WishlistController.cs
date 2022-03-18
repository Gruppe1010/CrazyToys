﻿using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
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
    public class WishlistController : RenderController
    {
        private readonly ToyDbService _toyDbService;
        private readonly ISessionService _sessionService;



        public WishlistController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor,
            ToyDbService toyDbService, ISessionService sessionService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sessionsUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            List<ShopToyDTO> wishlistToys = new List<ShopToyDTO>();

            foreach (var entry in sessionsUser.Wishlist)
            {
                Toy toy = await _toyDbService.GetById(entry);
                wishlistToys.Add(toy.ConvertToShopToyDTO());
            }

            ViewData["wishlistToys"] = wishlistToys;
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
