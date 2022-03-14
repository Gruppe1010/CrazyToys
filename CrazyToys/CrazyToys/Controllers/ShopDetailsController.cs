using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShopDetailsController : RenderController
    {
        private readonly ToyDbService _toyDbService;
 
        public ShopDetailsController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor, ToyDbService toyDbService) 
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _toyDbService = toyDbService;
        }

        [HttpGet]
        //[FromQuery bruges til at tage imod query parameter fra url]
        public async Task<IActionResult> Index([FromQuery(Name = "id")] string id)
        {
            var toy = await _toyDbService.GetById(id);

            ViewData["Toy"] = toy;
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
