using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class HomeController : RenderController
    {
        private readonly ISessionService _sessionService;
        private readonly IHangfireService _hangfireService;

        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, IHangfireService hangfireService, ISessionService sessionsService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _sessionService = sessionsService;
        }

        //
        // Summary:
        //     Before the controller executes we will handle redirects and not founds

        public override IActionResult Index()
        {
            // Session
            SessionUser sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);

            sessionUser.Cart.Add(new SelectedToy());

            ViewData["CartQuantity"] = 13;//sessionUser.Cart.Count;
            string test = "gruppe10";

            ViewData["Test"] = test;//JsonConvert.SerializeObject(test);

            ViewBag.Current = "Home";

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
