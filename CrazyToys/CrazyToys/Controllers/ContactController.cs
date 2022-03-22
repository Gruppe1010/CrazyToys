using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ContactController : RenderController
    {
 
        public ContactController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {

        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index()
        {

            ViewData["Title"] = "Kontakt";
            ViewBag.Current = "Contact";

            return CurrentTemplate(CurrentPage);
        }
    }
}





   
