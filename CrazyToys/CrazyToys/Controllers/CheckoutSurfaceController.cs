using CrazyToys.Interfaces;
using CrazyToys.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Filters;
using Umbraco.Cms.Web.Website.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class CheckoutSurfaceController : SurfaceController
    {
        private readonly ISessionService _sessionService;

        public CheckoutSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory,
            ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, ISessionService sessionService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _sessionService = sessionService;

        }


        [HttpPost]
        public IActionResult Submit(CheckoutUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            _sessionService.GetNewOrExistingSessionUser(HttpContext);
            // Work with form data here
            //TODO Lav en Order entity Og få hangfire til at kalde en metode der sender email til kunde med en ordre bekfræftelse

            var noget = model.Firstname;

            return Redirect("https://localhost:44325/order-confirmation");
        }
    }
}