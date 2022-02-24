using CrazyToys.Entities.Entities;
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
    public class ShopDetailsController : RenderController
    {
        private readonly IHangfireService _hangfireService;

        
        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;
        private readonly IEntityCRUD<Colour> _colourDbService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;

        public ShopDetailsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "id")] string id)
        {
            Console.WriteLine("INDEEEEEX: " + id);
            return CurrentTemplate(CurrentPage);
        }

        /*
        public override IActionResult Index()
        {
            Console.WriteLine("hej");
            return CurrentTemplate(CurrentPage);    
        }
        */

        /*
        //shop-details/product/id
        public IActionResult Product(string? id)
        {
            
            Console.WriteLine("DEEEETAILS med id: " + id);

            return View("~/Views/ShopDetails.cshtml");
        }
        */


    }
}





   
