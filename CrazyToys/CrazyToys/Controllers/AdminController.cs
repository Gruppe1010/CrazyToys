using CrazyToys.Entities.DTOs.OrderDTOs;
using CrazyToys.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{

    //[Route("[controller]/[action]")]

    public class AdminController : RenderController
    {

        private readonly AdminService _adminService;

        public AdminController(
            ILogger<RenderController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor,
            AdminService adminService
            ) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _adminService = adminService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index()
        {
            List<OrderDTO> orderDTOs = await _adminService.GetAdminOrderDTO();

            if(orderDTOs != null)
            {
                orderDTOs = orderDTOs.OrderByDescending(o => o.OrderNumber).ToList();
            }
            else
            {
                orderDTOs = new List<OrderDTO>();
            }

            ViewData["OrderDTOs"] = orderDTOs;

            return CurrentTemplate(CurrentPage);
        }

     

    }
}
