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
        private readonly ToyDbService _toyDbService;
 
        public ShopDetailsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, ToyDbService toyDbService) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string id)
        {
            Console.WriteLine("INDEEEEEX: " + id);
            var toy = await _toyDbService.GetById(id);

            ViewData["Toy"] = toy;
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





   
