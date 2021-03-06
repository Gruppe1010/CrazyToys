using CrazyToys.Entities.DTOs;
using CrazyToys.Entities.DTOs.FacetDTOs;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.ProductDbServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;
        private readonly CategoryDbService _categoryDbService;


        public ShopController(
            ILogger<HomeController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor,
            IHangfireService hangfireService,
            ISearchService<SolrToy> solrToyService,
            ISessionService sessionService,
            ToyDbService toyDbService,
            IEntityCRUD<Brand> brandDbService,
            IEntityCRUD<ColourGroup> colourGroupDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService,
            IEntityCRUD<PriceGroup> priceGroupDbService,
            CategoryDbService categoryDbService)
            : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _hangfireService = hangfireService;
            _solrToyService = solrToyService;
            _sessionService = sessionService;

            _toyDbService = toyDbService;
            _brandDbService = brandDbService;
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
            string url = _solrToyService.CreateSearchUrl(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups, pageNumber, search, sort);
            dynamic content = await _solrToyService.GetContent(url);

            Dictionary<int, List<ShopToyDTO>> toyDict = _solrToyService.GetToysFromContent(content);
            Dictionary<string, Dictionary<string, int>> facetFieldDict = _solrToyService.GetFacetFieldsFromContent(content);

         
            int numFound = toyDict.ElementAt(0).Key;
            List<ShopToyDTO> shopToyDTOs = toyDict.ElementAt(0).Value;

            List<CategoryDTO> categoryDTOs = facetFieldDict.ContainsKey("categories")
               ? await CreateCategoryDTOList(facetFieldDict["categories"], facetFieldDict.ContainsKey("subCategory_str") ? facetFieldDict["subCategory_str"] : null)
               : new List<CategoryDTO>();

            List<BrandDTO> brandDTOs = facetFieldDict.ContainsKey("brand")
                ? await CreateBrandDTOList(facetFieldDict["brand"])
                : new List<BrandDTO>();

            List<PriceGroupDTO> priceGroupDTOs = facetFieldDict.ContainsKey("priceGroup")
                ? await CreatePriceGroupDTOList(facetFieldDict["priceGroup"])
                : new List<PriceGroupDTO>();

            List<AgeGroupDTO> ageGroupDTOs = facetFieldDict.ContainsKey("ageGroupIntervals")
               ? await CreateAgeGroupDTOList(facetFieldDict["ageGroupIntervals"])
               : new List<AgeGroupDTO>();

            List<ColourGroupDTO> colourGroupDTOs = facetFieldDict.ContainsKey("colourGroups")
                ? await CreateColourGroupDTOList(facetFieldDict["colourGroups"])
                : new List<ColourGroupDTO>();

            var sessionUser = _sessionService.GetNewOrExistingSessionUser(HttpContext);
            HashSet<string> wishlistToys = sessionUser.Wishlist;

            ViewData["NumFound"] = numFound;
            ViewData["ShopToyDTOs"] = shopToyDTOs;
            ViewData["WishlistToys"] = wishlistToys;
            ViewData["ParamsDict"] = CreateDictFromParams(categories, subCategory, brand, priceGroup, ageGroupIntervals, colourGroups);
            ViewData["PageNumber"] = pageNumber == 0 ? 1 : pageNumber;
            ViewData["Search"] = search;

            ViewData["CategoryDTOs"] = categoryDTOs;
            ViewData["BrandDTOs"] = brandDTOs;
            ViewData["PriceGroups"] = priceGroupDTOs;
            ViewData["AgeGroupDTOs"] = ageGroupDTOs;
            ViewData["ColourGroupDTOs"] = colourGroupDTOs;

            ViewBag.Current = "Butik";

            // return a 'model' to the selected template/view for this page.
            return CurrentTemplate(CurrentPage);
        }

        private async Task<List<BrandDTO>> CreateBrandDTOList(Dictionary<string, int> brandFacets)
        {
            List<BrandDTO> brandDTOs = new List<BrandDTO>();

            List<Brand> brands = await _brandDbService.GetAll();

            if (brandFacets != null)
            {
                foreach (Brand brand in brands)
                {
                    if (brandFacets.ContainsKey(brand.Name))
                    {
                        brandDTOs.Add(new BrandDTO(brand.ID, brand.Name, brandFacets[brand.Name]));
                    }
                }
            }

            return brandDTOs.OrderBy(b => b.Name).ToList();
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
                                if (subCategoryGroupFacets.ContainsKey(subCategory.Name))
                                {
                                    categoryDTO.SubCategoryDTOs.Add(new SubCategoryDTO(subCategory.ID, subCategory.Name, subCategoryGroupFacets[subCategory.Name]));
                                }
                            }
                            categoryDTO.SubCategoryDTOs = categoryDTO.SubCategoryDTOs.OrderBy(s => s.Name).ToList();
                        }


                    }
                }
            }

            return categoryDTOs.OrderBy(c => c.Name).ToList();
        }

        public async Task<List<PriceGroupDTO>> CreatePriceGroupDTOList(Dictionary<string, int> priceGroupFacets)
        {
            List<PriceGroupDTO> priceGroupDTOs = new List<PriceGroupDTO>();

            List<PriceGroup> priceGroups = await _priceGroupDbService.GetAll();

            if (priceGroupFacets != null)
            {
                foreach (PriceGroup priceGroup in priceGroups)
                {

                    if (priceGroupFacets.ContainsKey(priceGroup.Interval.ToLower()))
                    {
                        priceGroupDTOs.Add(new PriceGroupDTO(priceGroup.ID, priceGroup.Interval, priceGroupFacets[priceGroup.Interval.ToLower()]));
                    }
                }
            }

            return priceGroupDTOs.OrderBy(c => c.Interval).ToList();
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
            string colour)
        {

            var dict = new Dictionary<string, HashSet<string>>();

            AddParamToDict(dict, category);
            AddParamToDict(dict, subCategory);
            AddParamToDict(dict, brand);
            AddParamToDict(dict, price);
            AddParamToDict(dict, ageGroup);
            AddParamToDict(dict, colour);

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
