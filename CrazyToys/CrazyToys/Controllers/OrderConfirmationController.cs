using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces;
using CrazyToys.Services;
using CrazyToys.Services.SalesDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
        private readonly SalesService _salesService;
        private readonly IHangfireService _hangfireService;
        private readonly ISessionService _sessionService;


        public OrderConfirmationController(
            ILogger<HomeController> logger, 
            ICompositeViewEngine compositeViewEngine,
            SalesService salesDataService,
            IHangfireService hangfireService,
            ISessionService sessionService,

            IUmbracoContextAccessor umbracoContextAccessor, OrderDbService orderDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _orderDbService = orderDbService;
            _salesService = salesDataService;
            _hangfireService = hangfireService;
            _sessionService = sessionService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index([FromQuery(Name = "orderNumber")] int orderNumber)
        {
            // find den nyoprettede ordre
            Order order = await _orderDbService.GetByOrderNumber(orderNumber);

            if(order != null) // 
            {
                // Opdater SolrToys soldAmount
                _salesService.UpdateSoldAmountInSolrToys(order.OrderLines);

                // lav liste med toys som skal vises i orderConfirmation
                List<ShoppingCartToyDTO> orderConfirmationToyList = await _salesService.ConvertOrderLinesToShoppingCartToyDTOs(order.OrderLines);

                // Send email til bruger
                OrderConfirmationDTO orderConfirmationDTO = order.ConvertToOrderConfirmationDTO(orderConfirmationToyList);
                _hangfireService.CreateOrderConfirmationJob(orderConfirmationDTO);

                // tilføjer Approved StatusType til ordren
                await _salesService.AddStatusType(order, 2);

                // slet cart på sessionUser
                SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
                sessionUser.Cart.Clear();
                _sessionService.Update(HttpContext, sessionUser);
            }

            ViewData["IsAnOrder"] = order != null;
            ViewData["Order"] = order;

            return CurrentTemplate(CurrentPage);
        }
    }
    
}





   
