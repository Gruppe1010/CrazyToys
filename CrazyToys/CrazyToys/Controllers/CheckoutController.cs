
using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
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
    public class CheckoutController : RenderController
    {
        private readonly ISessionService _sessionService;
        private readonly ToyDbService _toyDbService;

        public CheckoutController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor,
            ISessionService sessionService, ToyDbService toyDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _sessionService = sessionService;
            _toyDbService = toyDbService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index()
        {
            var sessionsUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            List<ShoppingCartToyDTO> shoppingCartToytDTOs = new List<ShoppingCartToyDTO>();

            foreach (var entry in sessionsUser.Cart)
            {
                Toy toy = await _toyDbService.GetById(entry.Key);
                shoppingCartToytDTOs.Add(toy.ConvertToShoppingCartToyDTO(entry.Value));
            }

            ViewBag.Current = "Tjek Ud";
            ViewData["ShoppingCartToytDTOs"] = shoppingCartToytDTOs;

            return CurrentTemplate(CurrentPage);
        }
    }
}
