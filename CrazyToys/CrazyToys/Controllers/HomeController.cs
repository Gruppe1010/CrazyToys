using CrazyToys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
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

        public override IActionResult Index()
        {
            ViewBag.Current = "Forside";


            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }
    }
}





   
