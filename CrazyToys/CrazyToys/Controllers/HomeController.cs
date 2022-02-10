using CrazyToys.Interfaces;
using CrazyToys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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

            //_  = _icecatDataService.getSingleProduct("15111", "BARBIEKS55");
            _ = _icecatDataService.getSingleProduct("5669", "HASB9940EU60");

            

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
