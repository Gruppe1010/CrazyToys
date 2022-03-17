using CrazyToys.Entities.DTOs;
using CrazyToys.Interfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShoppingCartController : RenderController
    {
        private readonly ToyDbService _toyDbService;
        private readonly ISessionService _sessionService;



        public ShoppingCartController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, 
            ToyDbService toyDbService, ISessionService sessionService) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
            _sessionService = sessionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var sessionsUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            List<ShoppingCartToyDTO> shoppingCartToytDTOs = null;

            if(sessionsUser.Cart.Count > 0)
            {

            }

            ViewData["ShoppingCartToytDTOs"] = shoppingCartToytDTOs;
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
