using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
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
        private readonly CategoryDbService _categoryDbService;
        private readonly ISessionService _sessionService;
        private readonly IRecommendationService _recommendationService;


        public ShopDetailsController(
            ILogger<RenderController> logger, 
            ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, 
            ToyDbService toyDbService,
            ISessionService sessionService,
            IRecommendationService recommendationService,
            CategoryDbService categoryDbService
            ) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
            _categoryDbService = categoryDbService;
            _sessionService = sessionService;
            _recommendationService = recommendationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string id)
        {
            var toy = await _toyDbService.GetById(id);

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            HashSet<string> wishlistToys = sessionUser.Wishlist;

            List<ShopToyDTO> relatedToys = await _recommendationService.GetRelatedToys(id, 8);

            List<Category> categories = await _categoryDbService.GetAllFromToy(toy);
            List<ShopToyDTO> mostPopularToys = await _recommendationService.GetMostPopularToys(toy, 8);

            ViewData["Toy"] = toy;
            ViewData["WishlistToys"] = wishlistToys;
            ViewData["RelatedToys"] = relatedToys;
            ViewData["MostPopularToys"] = mostPopularToys;
            ViewData["Category"] = categories.Count > 0 ? categories[0] : null;

            return CurrentTemplate(CurrentPage);
        }
    }
}


