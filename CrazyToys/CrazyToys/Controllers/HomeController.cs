using CrazyToys.Entities.Models.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class HomeController : RenderController
    {

        private readonly IProductDataService _icecatDataService;

        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, IProductDataService icecatDataService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _icecatDataService = icecatDataService;
        }

        public override IActionResult Index()
        {
            string test = "gruppe10";

            ViewData["Test"] = test;//JsonConvert.SerializeObject(test);


            Toy toy = Task.Run(async () => await _icecatDataService.GetSingleProduct("15111", "BARBIEKS55")).Result;
            Toy toy1 = Task.Run(async () => await _icecatDataService.GetSingleProduct("5669", "HASB9940EU60")).Result;

            //_ = _icecatDataService.GetSingleProduct("15111", "BARBIEKS55");
            //_ = _icecatDataService.GetSingleProduct("5669", "HASB9940EU60");



            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
