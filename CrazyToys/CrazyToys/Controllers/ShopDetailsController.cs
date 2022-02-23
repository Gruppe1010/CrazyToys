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
    public class ShopDetailsController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IHangfireService _hangfireService;

        
        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;
        private readonly IEntityCRUD<Colour> _colourDbService;
        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;

       
        public ShopDetailsController(IHangfireService hangfireService, IEntityCRUD<Brand> brandDbService, IEntityCRUD<Category> categoryDbService, 
            IEntityCRUD<Colour> colourDbService, ToyDbService toyDbService, IEntityCRUD<AgeGroup> ageGroupDbService)
        {
            _hangfireService = hangfireService;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _colourDbService = colourDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
        }

        public IActionResult Index(string? id)
        {
            Console.WriteLine("INDEEEEEX");
            return View("~/Views/ShopDetails.cshtml");
        }
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





   
