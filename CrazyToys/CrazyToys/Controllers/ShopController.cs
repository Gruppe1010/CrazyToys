using CrazyToys.Entities.Entities;
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

        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;




        public ShopController(
            ILogger<HomeController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            ToyDbService toyDbService,
            IEntityCRUD<ColourGroup> colourGroupDbService, 
            IEntityCRUD<AgeGroup> ageGroupDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService
          )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _solrToyService = solrToyService;

            _toyDbService = toyDbService;
            _colourGroupDbService = colourGroupDbService;
            _ageGroupDbService = ageGroupDbService;
            _priceGroupDbService = priceGroupDbService;
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
            //List<string> priceGroups = _solrToyService.GetPriceGroupFacet();

            var priceGroupTask = _priceGroupDbService.GetAll();
            priceGroupTask.Wait();
            List<PriceGroup> priceGroups = priceGroupTask.Result;

            var colourGroupTask = _colourGroupDbService.GetAll();
            colourGroupTask.Wait();
            List<ColourGroup> colourGroups = colourGroupTask.Result;

            var ageGroupTask = _ageGroupDbService.GetAll();
            ageGroupTask.Wait();
            List<AgeGroup> ageGroupList = ageGroupTask.Result;

            var toys = await _toyDbService.GetAllWithRelations();

            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroupList.OrderBy(a => a.Interval).ToList();
            ViewData["PriceGroups"] = priceGroups.OrderBy(a => a.Interval).ToList();
            ViewData["Brands"] = brandDict;
            ViewData["ColourGroups"] = colourGroups.OrderBy(a => a.Name).ToList();
            ViewData["Toys"] = toys;

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
