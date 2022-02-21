using CrazyToys.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class HomeController : RenderController
    {

        //private readonly IProductDataService _icecatDataService;
        private readonly IHangfireService _hangfireService;



        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, IHangfireService hangfireService)// IProductDataService icecatDataService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            //_icecatDataService = icecatDataService;
            _hangfireService = hangfireService;
        }

        //
        // Summary:
        //     Before the controller executes we will handle redirects and not founds

        /*
        public HomeController(ILogger<HomeController> logger, ICompositeViewEngine compositeViewEngine, 
            IUmbracoContextAccessor umbracoContextAccessor, IHangfireService hangfireService)// IProductDataService icecatDataService)
            //: base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _logger = logger;
            _compositeViewEngine = compositeViewEngine;
            _umbracoContextAccessor = umbracoContextAccessor;
            _hangfireService = hangfireService;

            //_icecatDataService = icecatDataService;
            _hangfireService = hangfireService;

        }
        */


        public override IActionResult Index()
        {
            string test = "gruppe10";

            ViewData["Test"] = test;//JsonConvert.SerializeObject(test);

            
            string indexUrl = "https://data.Icecat.biz/export/freexml/EN/files.index.xml";
            string dailyUrl = "https://data.Icecat.biz/export/freexml/EN/daily.index.xml";

            /*
            
            var getindextask = _hangfireService.GetProductsFromIcecat(dailyUrl);
            getindextask.Wait();
            */
            


            // hent alle PRODUKTER
            /*
            var getAllProductsTask = _hangfireService.GetAllProducts();
            getIndexTask.Wait();
            */




            //var result = getIndexTask.Result;

            //await _hangfireService.GetIndex();
            /*
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("23442", "0401050")).Result;
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("5669", "HASB9940EU60")).Result;
            _ = Task.Run(async () => await _icecatDataService.GetSingleProduct("15111", "GXB29")).Result;
            */



            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }


      


    }
}





   
