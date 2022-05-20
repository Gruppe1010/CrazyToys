using CrazyToys.Entities.DTOs;
using CrazyToys.Interfaces;
using CrazyToys.Services.ProductDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShopDetailsController : RenderController
    {
        private readonly ToyDbService _toyDbService;
        private readonly ISessionService _sessionService;
        private readonly IRecommendationService _recommendationService;


        public ShopDetailsController(
            ILogger<RenderController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            ToyDbService toyDbService,
            ISessionService sessionService,
            IRecommendationService recommendationService
            ) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
            _sessionService = sessionService;
            _recommendationService = recommendationService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string id)
        {
            var toy = await _toyDbService.GetById(id);

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            HashSet<string> wishlistToys = sessionUser.Wishlist;

            List<string> relatedToyIds = await _recommendationService.FindRelatedToyIds(id);

            List<ShopToyDTO> relatedToys = _recommendationService.GetRelatedShopToyDTOs(relatedToyIds, 8);


            ViewBag.Current = "Legetøjs Detaljer";
            ViewData["Toy"] = toy;
            ViewData["WishlistToys"] = wishlistToys;
            ViewData["RelatedToys"] = relatedToys;
            return CurrentTemplate(CurrentPage);
        }
    }
}
