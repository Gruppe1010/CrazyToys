﻿using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
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
        private readonly ISessionService _sessionService;

        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;




        public ShopController(
            ILogger<HomeController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            ISessionService sessionService,
            ToyDbService toyDbService,
            IEntityCRUD<ColourGroup> colourGroupDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService, 
            IEntityCRUD<Category> categoryDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _solrToyService = solrToyService;
            _sessionService = sessionService;

            _toyDbService = toyDbService;
            _colourGroupDbService = colourGroupDbService;
            _ageGroupDbService = ageGroupDbService;
            _priceGroupDbService = priceGroupDbService;
            _categoryDbService = categoryDbService;
        }

        //
        // Summary:
        //     Before the controller executes we will handle redirects and not founds


        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery(Name = "categories")] string categories,
            [FromQuery(Name = "subCategory")] string subCategory,
            [FromQuery(Name = "brand")] string brand,
            [FromQuery(Name = "priceGroup")] string priceGroup,
            [FromQuery(Name = "ageGroupIntervals")] string ageGroupIntervals,
            [FromQuery(Name = "colourGroups")] string colourGroups,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "search")] string search,
            [FromQuery(Name = "sort")] string sort)
        {

            Dictionary<int, List<ShopToyDTO>> dict = await _solrToyService.GetToysForSinglePage(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, pageNumber, search, sort);

            int numFound = dict.ElementAt(0).Key;
            List<ShopToyDTO> shopToyDTOs = dict.ElementAt(0).Value;

            SortedDictionary<string, int> brandDict = _solrToyService.GetBrandFacet();
            SortedDictionary<string, int> categoryDict = _solrToyService.GetCategoryFacet();

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            HashSet<string> wishlistToys = sessionUser.Wishlist;

            List<PriceGroup> priceGroups = await _priceGroupDbService.GetAll();
            List<ColourGroup> colourGroupList = await _colourGroupDbService.GetAll();
            List<AgeGroup> ageGroupList = await _ageGroupDbService.GetAll();
            List<Category> categoryList = await _categoryDbService.GetAllWithRelations();

            ViewData["NumFound"] = numFound;
            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroupList.OrderBy(a => a.Interval).ToList();
            ViewData["PriceGroups"] = priceGroups.OrderBy(p => p.Interval).ToList();
            ViewData["CategoryList"] = categoryList.OrderBy(c => c.Name).ToList();
            ViewData["Brands"] = brandDict;
            ViewData["ColourGroups"] = colourGroupList.OrderBy(c => c.Name).ToList();
            ViewData["ShopToyDTOs"] = shopToyDTOs;
            ViewData["ParamsDict"] = CreateDictFromParams(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, search);
            ViewData["PageNumber"] = pageNumber == 0 ? 1 : pageNumber;
            ViewData["WishlistToys"] = wishlistToys;

            ViewBag.Current = "Butik";

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }


        public Dictionary<string, HashSet<string>> CreateDictFromParams(
            string category, // category.Spil
            string subCategory,
            string brand,
            string price,
            string ageGroup,
            string colour,
            string search)
        {

            var dict = new Dictionary<string, HashSet<string>>();

            AddParamToDict(dict, category);
            AddParamToDict(dict, subCategory);
            AddParamToDict(dict, brand);
            AddParamToDict(dict, price);
            AddParamToDict(dict, ageGroup);
            AddParamToDict(dict, colour);
            AddParamToDict(dict, search);

            return dict;
        }

        // param == category.Spil
        public void AddParamToDict(Dictionary<string, HashSet<string>> dict, string param)
        {
            if (param != null)
            {
                string[] values = param.Split('.'); // ["category", "Spil"]

                string type = values[0]; // category

                dict.Add(type, new HashSet<string>());

                for (int i = 1; i < values.Length; i++)
                {
                    dict[type].Add(values[i]);
                }
            }
        }
    }
}





   
