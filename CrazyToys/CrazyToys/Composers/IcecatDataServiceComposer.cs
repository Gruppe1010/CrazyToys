using CrazyToys.Interfaces;
using CrazyToys.Services;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Extensions;


namespace CrazyToys.Web.Composers
{
    /* Composer bliver brugt til at injecte dependencies i controllers
     * Så fordi vi i HomeControlleren har et field med en IProductDataService som skal være en IcecatDataService
     *      så skal vi sige at den skal injecte IcecatDataService derind
     */
    public class IcecatDataServiceComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            //builder.Services.AddUnique<IProductDataService, IcecatDataService>();
            //builder.Services.
            //builder.Services.Add(IProductDataService);

        }
    }
}





  
