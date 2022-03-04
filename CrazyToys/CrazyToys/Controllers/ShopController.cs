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
        private readonly ISearchService<SolrToy> _solrService;


        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;
        private readonly IEntityCRUD<Colour> _colourDbService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
       

        public ShopController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, IHangfireService hangfireService,
            IEntityCRUD<Brand> brandDbService, IEntityCRUD<Category> categoryDbService,
            IEntityCRUD<Colour> colourDbService, ToyDbService toyDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService, ISearchService<SolrToy> solrService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _colourDbService = colourDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
            _solrService = solrService;
        }

        //
        // Summary:
        //     Before the controller executes we will handle redirects and not founds

        public override IActionResult Index()
        {

            Dictionary<string, int> brandDict = _solrService.GetBrandFacets();
            Dictionary<string, int> categoryDict = _solrService.GetCategoryFacets();




            var getAllAgeGroupsTask = _ageGroupDbService.GetAll();
            getAllAgeGroupsTask.Wait();
            List<AgeGroup> ageGroups = getAllAgeGroupsTask.Result;
            ageGroups.Sort((x, y) => x.Interval[0].CompareTo(y.Interval[0]));

            var getAllColoursTask = _colourDbService.GetAll();
            getAllColoursTask.Wait();
            List<Colour> colours = getAllColoursTask.Result;

            
            var getAllToysTask = _toyDbService.GetAllWithRelations();
            getAllToysTask.Wait();
            
            List<Toy> toys = getAllToysTask.Result;

            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroups;
            ViewData["Brands"] = brandDict;
            ViewData["Colours"] = colours;
            ViewData["Toys"] = toys;


            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
