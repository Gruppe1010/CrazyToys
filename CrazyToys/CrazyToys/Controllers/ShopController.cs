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
        private readonly ISearchService<SolrPriceGroup> _solrPriceGroupService;



        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;
        private readonly IEntityCRUD<Colour> _colourDbService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
       

        public ShopController(
            ILogger<HomeController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            ISearchService<SolrPriceGroup> solrPriceGroupService,
            IEntityCRUD<Brand> brandDbService, 
            IEntityCRUD<Category> categoryDbService,
            IEntityCRUD<Colour> colourDbService, 
            ToyDbService toyDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService
          )
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _colourDbService = colourDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
            _solrToyService = solrToyService;
            _solrPriceGroupService = solrPriceGroupService;

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
            List<string> coloursList = _solrToyService.GetColourFacet();
            var priceGroups = _solrPriceGroupService.GetAll();


            var toys = await _toyDbService.GetAllWithRelations();

            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroupsList.OrderBy(a => a).ToList();
            ViewData["Brands"] = brandDict;
            ViewData["Colours"] = coloursList.OrderBy(a => a).ToList();
            ViewData["Toys"] = toys;


            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
