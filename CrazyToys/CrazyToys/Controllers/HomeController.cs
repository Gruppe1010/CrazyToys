
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

        //private readonly IProductDataService _icecatDataService;
        private readonly IHangfireService _hangfireService;


        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, IHangfireService hangfireService)// IProductDataService icecatDataService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //_icecatDataService = icecatDataService;
            _hangfireService = hangfireService;

        }

        public override IActionResult Index()
        {
            string test = "gruppe10";

            ViewData["Test"] = test;//JsonConvert.SerializeObject(test);

            
            _hangfireService.GetIndex();
            /*
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("23442", "0401050")).Result;
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("5669", "HASB9940EU60")).Result;
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("15111", "GXB29")).Result;
            */



            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
