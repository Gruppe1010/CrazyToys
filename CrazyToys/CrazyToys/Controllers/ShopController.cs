﻿using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShopController : RenderController
    {
        private readonly IHangfireService _hangfireService;
        private readonly ISearchService<SolrToy> _solrToyService;

        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly ToyDbService _toyDbService;
       

        public ShopController(
            ILogger<HomeController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            IEntityCRUD<ColourGroup> colourGroupDbService, 
            ToyDbService toyDbService
          )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _colourGroupDbService = colourGroupDbService;
            _toyDbService = toyDbService;
            _solrToyService = solrToyService;
        }

        //
        // Summary:
        //     Before the controller executes we will handle redirects and not founds


        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery(Name = "category")] string category, // én
            [FromQuery(Name = "subCategory")] string subCategory, // én
            [FromQuery(Name = "brands")] string brands,
            [FromQuery(Name = "price")] string price,
            [FromQuery(Name = "ageGroups")] string ageGroups,
            [FromQuery(Name = "colours")] string colours)
        {
            SortedDictionary<string, int> brandDict = _solrToyService.GetBrandFacet();
            SortedDictionary<string, int> categoryDict = _solrToyService.GetCategoryFacet();
            List<string> ageGroupsList= _solrToyService.GetAgeGroupsFacet();
            var colourGroupTask = _colourGroupDbService.GetAll();
            colourGroupTask.Wait();
            List<ColourGroup> colourGroups = colourGroupTask.Result;
            //List<string> coloursList = _solrToyService.GetColourFacet();
            List<string> priceGroupsList = _solrToyService.GetPriceGroupFacet();

            var toys = await _toyDbService.GetAllWithRelations();

            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroupsList.OrderBy(a => a).ToList();
            ViewData["PriceGroups"] = priceGroupsList.OrderBy(a => a).ToList();
            ViewData["Brands"] = brandDict;
            ViewData["ColourGroups"] = colourGroups.OrderBy(a => a.Name).ToList();
            ViewData["Toys"] = toys;

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
