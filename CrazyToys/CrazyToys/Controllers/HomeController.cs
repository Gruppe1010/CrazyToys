using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    /*
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    */

    public class HomeController : RenderController
    {
        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }

        public override IActionResult Index()
        {
            // you are in control here!
            System.Console.WriteLine("Vi er indeeeeeeeeeeeeeee");

            string test = "gruppe10";

            ViewData["Test"] = test;//JsonConvert.SerializeObject(test);

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
