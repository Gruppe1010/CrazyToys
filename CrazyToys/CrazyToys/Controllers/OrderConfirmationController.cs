using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Services.SalesDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    
    public class OrderConfirmationController : RenderController
    {

        private readonly OrderDbService _orderDbService;

        public OrderConfirmationController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, OrderDbService orderDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _orderDbService = orderDbService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index([FromQuery(Name = "orderNumber")] int orderNumber)
        {

            Order order = await _orderDbService.GetByOrderNumber(orderNumber);

            ViewData["IsAnOrder"] = order != null;
            ViewData["Order"] = order;

            return CurrentTemplate(CurrentPage);
        }
    }
    
}





   
