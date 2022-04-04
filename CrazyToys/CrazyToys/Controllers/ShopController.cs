using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.FacetDTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace CrazyToys.Web.Controllers
{
    public class ShopController : RenderController
    {
        private readonly IHangfireService _hangfireService;
        private readonly ISearchService<SolrToy> _solrToyService;
        private readonly ISessionService _sessionService;

        private readonly ToyDbService _toyDbService;
        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;

        public ShopController(
            ILogger<HomeController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            ISessionService sessionService,
            ToyDbService toyDbService,
            IEntityCRUD<ColourGroup> colourGroupDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService,
            IEntityCRUD<Category> categoryDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _solrToyService = solrToyService;
            _sessionService = sessionService;

            _toyDbService = toyDbService;
            _colourGroupDbService = colourGroupDbService;
            _ageGroupDbService = ageGroupDbService;
            _priceGroupDbService = priceGroupDbService;
            _categoryDbService = categoryDbService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(
            [FromQuery(Name = "categories")] string categories,
            [FromQuery(Name = "subCategory")] string subCategory,
            [FromQuery(Name = "brand")] string brand,
            [FromQuery(Name = "priceGroup")] string priceGroup,
            [FromQuery(Name = "ageGroupIntervals")] string ageGroupIntervals,
            [FromQuery(Name = "colourGroups")] string colourGroups,
            [FromQuery(Name = "p")] int pageNumber,
            [FromQuery(Name = "search")] string search,
            [FromQuery(Name = "sort")] string sort)
        {

            dynamic content = await _solrToyService.GetContent(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, pageNumber, search, sort);


            Dictionary<int, List<ShopToyDTO>> toyDict = _solrToyService.GetToysFromContent(content);
            Dictionary<string, Dictionary<string, int>> facetFieldDict = _solrToyService.GetFacetFieldsFromContent(content);

            // farvekoder skal hentes fra db også
            //Dictionary<int, List<ShopToyDTO>> dict = await _solrToyService.GetToysForSinglePage(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, pageNumber, search, sort);



            int numFound = toyDict.ElementAt(0).Key;
            List<ShopToyDTO> shopToyDTOs = toyDict.ElementAt(0).Value;

            List<CategoryDTO> categoryDTOs = facetFieldDict.ContainsKey("categories")
               ? await CreateCategoryDTOList(facetFieldDict["categories"], facetFieldDict.ContainsKey("subCategory") ? facetFieldDict["subCategory"] : null)
               : new List<CategoryDTO>();

            List<AgeGroupDTO> ageGroupDTOs = facetFieldDict.ContainsKey("ageGroupIntervals")
               ? await CreateAgeGroupDTOList(facetFieldDict["ageGroupIntervals"])
               : new List<AgeGroupDTO>();

            List<ColourGroupDTO> colourGroupDTOs = facetFieldDict.ContainsKey("colourGroups")
                ? await CreateColourGroupDTOList(facetFieldDict["colourGroups"])
                : new List<ColourGroupDTO>();




            //SortedDictionary<string, int> brandDict = _solrToyService.GetBrandFacet();
            //SortedDictionary<string, int> categoryDict = _solrToyService.GetCategoryFacet();
            //SortedDictionary<string, int> subCategoryDict = _solrToyService.GetSubCategoryFacet();




            /*
            List<PriceGroup> priceGroups = await _priceGroupDbService.GetAll();
            List<AgeGroup> ageGroupList = await _ageGroupDbService.GetAll();
            */

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            HashSet<string> wishlistToys = sessionUser.Wishlist;

            ViewData["NumFound"] = numFound;
            ViewData["Categories"] = facetFieldDict["categories"];
            ViewData["AgeGroupDTOs"] = ageGroupDTOs;
            ViewData["PriceGroups"] = facetFieldDict["priceGroup"];
            ViewData["CategoryDTOs"] = categoryDTOs; // TODO Vi skal hente fra Solr og Db og få ind i view
            ViewData["Brands"] = facetFieldDict["brand"];
            ViewData["ColourGroupDTOs"] = colourGroupDTOs;


            ViewData["ShopToyDTOs"] = shopToyDTOs;
            ViewData["ParamsDict"] = CreateDictFromParams(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, search);
            ViewData["PageNumber"] = pageNumber == 0 ? 1 : pageNumber;
            ViewData["WishlistToys"] = wishlistToys;

            /*
            ViewData["NumFound"] = numFound;
            ViewData["Categories"] = categoryDict;
            ViewData["AgeGroups"] = ageGroupList.OrderBy(a => a.Interval).ToList();
            ViewData["PriceGroups"] = priceGroups.OrderBy(p => p.Interval).ToList();
            ViewData["CategoryList"] = categoryList.OrderBy(c => c.Name).ToList();
            ViewData["Brands"] = brandDict;
            ViewData["ColourGroups"] = colourGroupList.OrderBy(c => c.Name).ToList();
      
            */

            ViewBag.Current = "Butik";

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }

        public async Task<List<CategoryDTO>> CreateCategoryDTOList(Dictionary<string, int> categoryGroupFacets, Dictionary<string, int> subCategoryGroupFacets)
        {
            List<CategoryDTO> categoryDTOs = new List<CategoryDTO>();

            List<Category> categories = await _categoryDbService.GetAllWithRelations();


            if (categories != null)
            {
                foreach (Category category in categories)
                {
                    if (categoryGroupFacets.ContainsKey(category.Name.ToLower()))
                    {
                        CategoryDTO categoryDTO = new CategoryDTO(category.ID, category.Name, categoryGroupFacets[category.Name.ToLower()]);
                        categoryDTOs.Add(categoryDTO);

                        if (subCategoryGroupFacets != null)
                        {
                            foreach (SubCategory subCategory in category.SubCategories)
                            {
                                if (subCategoryGroupFacets.ContainsKey(subCategory.Name.ToLower()))
                                {
                                    categoryDTO.SubCategoryDTOs.Add(new SubCategoryDTO(subCategory.ID, subCategory.Name, subCategoryGroupFacets[subCategory.Name.ToLower()]));
                                }
                            }
                            categoryDTO.SubCategoryDTOs = categoryDTO.SubCategoryDTOs.OrderBy(s => s.Name).ToList();
                        }


                    }
                }
            }

            return categoryDTOs.OrderBy(c => c.Name).ToList();
        }



        public async Task<List<AgeGroupDTO>> CreateAgeGroupDTOList(Dictionary<string, int> ageGroupFacets)
        {
            List<AgeGroupDTO> ageGroupDTOs = new List<AgeGroupDTO>();

            List<AgeGroup> ageGroups = await _ageGroupDbService.GetAll();

            if (ageGroupFacets != null)
            {
                foreach (AgeGroup ageGroup in ageGroups)
                {

                    if (ageGroupFacets.ContainsKey(ageGroup.Interval.ToLower()))
                    {
                        ageGroupDTOs.Add(new AgeGroupDTO(ageGroup.ID, ageGroup.Interval, ageGroupFacets[ageGroup.Interval.ToLower()]));
                    }
                }
            }

            return ageGroupDTOs.OrderBy(a => a.Interval).ToList();
        }


        public async Task<List<ColourGroupDTO>> CreateColourGroupDTOList(Dictionary<string, int> colourGroupFacets)
        {
            List<ColourGroupDTO> colourGroupDTOs = new List<ColourGroupDTO>();

            List<ColourGroup> colourGroups = await _colourGroupDbService.GetAll();

            if (colourGroupFacets != null)
            {
                foreach (ColourGroup colourGroup in colourGroups)
                {

                    if (colourGroupFacets.ContainsKey(colourGroup.Name.ToLower()))
                    {
                        colourGroupDTOs.Add(new ColourGroupDTO(colourGroup.ID, colourGroup.Name, colourGroup.ColourCode, colourGroupFacets[colourGroup.Name.ToLower()]));
                    }
                }
            }

            return colourGroupDTOs;
        }

        public Dictionary<string, HashSet<string>> CreateDictFromParams(
            string category, // category.Spil
            string subCategory,
            string brand,
            string price,
            string ageGroup,
            string colour,
            string search)
        {

            var dict = new Dictionary<string, HashSet<string>>();

            AddParamToDict(dict, category);
            AddParamToDict(dict, subCategory);
            AddParamToDict(dict, brand);
            AddParamToDict(dict, price);
            AddParamToDict(dict, ageGroup);
            AddParamToDict(dict, colour);
            AddParamToDict(dict, search);

            return dict;
        }

        // param == category.Spil
        public void AddParamToDict(Dictionary<string, HashSet<string>> dict, string param)
        {
            if (param != null)
            {
                string[] values = param.Split('.'); // ["category", "Spil"]

                string type = values[0]; // category

                dict.Add(type, new HashSet<string>());

                for (int i = 1; i < values.Length; i++)
                {
                    dict[type].Add(values[i]);
                }
            }
        }
    }
}
