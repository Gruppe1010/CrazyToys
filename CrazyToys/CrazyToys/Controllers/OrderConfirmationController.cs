using CrazyToys.Interfaces;
using CrazyToys.Services.SalesDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    
    public class OrderConfirmationController : RenderController
    {

        public OrderConfirmationController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {}

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public IActionResult Index([FromQuery(Name = "id")] string id)
        {
            ViewData["OrderNumber"] = id;

            return CurrentTemplate(CurrentPage);
        }
    }
    
}





   
