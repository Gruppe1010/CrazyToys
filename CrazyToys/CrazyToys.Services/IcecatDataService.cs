using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class IcecatDataService : IProductDataService
    {
        
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly IEntityCRUD<Category> _categoryDbService;
        private readonly IEntityCRUD<SubCategory> _subCategoryDbService;
        private readonly IEntityCRUD<Colour> _colourDbService;
        private readonly IEntityCRUD<Toy> _toyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private Random random;


        public IcecatDataService(IHttpClientFactory httpClientFactory, IEntityCRUD<Brand> brandDbService, IEntityCRUD<Category> categoryDbService,
            IEntityCRUD<SubCategory> subCategoryDbService, IEntityCRUD<Colour> colourDbService, IEntityCRUD<Toy> toyDbService, IEntityCRUD<AgeGroup> ageGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _subCategoryDbService = subCategoryDbService;
            _colourDbService = colourDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
            random = new Random();
        }

        /**
         * returns string[] - index 0 == username og index 1 er credentials
         */
        public string GetIcecatCredentials()
        {
            // TODO implementer lige noget sikkerhedsnoget her
            string username = "alphaslo";
            string password = "KJ6j1c9y8c2YwMq8GTjc";

            byte[] byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(byteArray);
        }


        public async Task<Toy> GetSingleProduct(string brandId, string productId)
        {
            Toy toy = null;

            // TODO tilføj sikkerhed ift. brugernavn
            string username = "alphaslo";
            string credentials = GetIcecatCredentials();

            Brand brand = await _brandDbService.GetById(brandId);
            if (brand != null)
            {
                string brandNameWithoutSpaces = brand.Name.Replace(" ", "%20");

                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                    HttpMethod.Get,
                    $"https://live.icecat.biz/api/?Username={username}&Language=dk&Brand={brandNameWithoutSpaces}&ProductCode={productId}")

                {
                    Headers =
                    {
                        { HeaderNames.Accept, "application/json" },
                        { HeaderNames.Authorization, $"Basic {credentials} " }
                    }
                };

                HttpClient httpClient = _httpClientFactory.CreateClient();
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    toy = new Toy();
                    toy.Brand = brand;
                    bool hasAgeGroup = false; // bruges til at sætte til "Ingens Aldersgruppe"-agegroupen, hvis ingen alder-presentationvalue fundet
                    var ageGroups = Task.Run(async () => await _ageGroupDbService.GetAll()).Result;
                    var categories = Task.Run(async () => await _categoryDbService.GetAll()).Result;

                    string jsonContent =
                        await httpResponseMessage.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(jsonContent);

                    string id = json["data"]["GeneralInfo"]["BrandPartCode"];
                    toy.ID = json["data"]["GeneralInfo"]["BrandPartCode"];
                    toy.Name = json["data"]["GeneralInfo"]["Title"];
                    toy.ShortDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["ShortSummaryDescription"];
                    toy.LongDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["LongSummaryDescription"];

                    string subCategoryId = json["data"]["GeneralInfo"]["Category"]["CategoryID"];
                    string subCategoryName = json["data"]["GeneralInfo"]["Category"]["Name"]["Value"];

                    toy.SubCategory = Task.Run(async () => await GetOrCreateSubCategory(subCategoryId, subCategoryName, categories)).Result;

                    string urlLow = json["data"]["Image"]["Pic500x500"];
                    string urlHigh = json["data"]["Image"]["HighPic"];
                    
                    Image image = new Image(urlLow, urlHigh, 0);
                    toy.Images.Add(image);

                    // tilføj resten af billeder som ligger i Gallery-key
                    foreach (dynamic img in json["data"]["Gallery"])
                    {
                        string galleryImageUrlLow = img["Pic500x500"];
                        string galleryImageUrlHigh = img["Pic"];
                        int galleryImageNo = img["No"];

                        Image galleryImage = new Image(galleryImageUrlLow, galleryImageUrlHigh, galleryImageNo);
                        toy.Images.Add(galleryImage);
                    }

                    // FeaturesGroups --> for hver på listen: ["Features"] for hver på listen: ["Feature"] if ["id"] = 1766 -->  item på ["Features"]["PresentationValue"]
                    // stringen skal splittes op i strings og så tilføjes som seperate værdier i colour tabellen, som så skal tilføjes som refs til toy
                    //string colourString = json["data"]["GeneralInfo"]["FeaturesGroups"]["Features"]["PresentationValue"];
                    string colourId = "1766";
                    string ageGroupYearsId = "24697";
                    string ageGroupMonthsId = "24019";

                    var featureGroups = json["data"]["FeaturesGroups"];

                    foreach (dynamic featuresGroup in json["data"]["FeaturesGroups"])
                    {
                        dynamic features = featuresGroup["Features"];

                        foreach (dynamic feature in features)
                        {

                            string featureId = feature["Feature"]["ID"];

                            if (featureId.Equals(colourId))
                            {
                                string presentationValue = feature["PresentationValue"];

                                string[] colours = presentationValue.Split(", ");

                                // for hver farve tilføj til toy-obj
                                Array.ForEach(colours, colourName => toy.Colours.Add(GetOrCreateColour(colourName)));

                            }
                            else if (featureId.Equals(ageGroupYearsId) || featureId.Equals(ageGroupMonthsId))
                            {
                                hasAgeGroup = true;
                                string presentationValue = feature["PresentationValue"];
                                toy.AgeGroup = presentationValue;

                                // uanset om det er måned eller år, så er det efter kommaet ligemeget, fordi udregningen bliver det samme
                                string age = presentationValue.Split(" ")[0].Split(".")[0];
                                int ageAsInt = Convert.ToInt32(age);

                                // hvis det er i måneder, skal det konverteres til år
                                if (featureId.Equals(ageGroupMonthsId))
                                {
                                    ageAsInt = ageAsInt / 12;
                                    age = ageAsInt.ToString();
                                }
                                // hvis det er den sidste aldersgruppe-kategori skal den findes manuelt ud fra "9"
                                if (ageAsInt > 8)
                                {
                                    foreach (AgeGroup ageGroup in ageGroups)
                                    {
                                        if (ageGroup.Interval.Contains("9"))
                                        {
                                            toy.AgeGroups.Add(ageGroup);
                                            break;
                                        }
                                    };
                                }
                                else // skal den findes ud fra selve tallet
                                {
                                    foreach (AgeGroup ageGroup in ageGroups)
                                    {
                                        if (ageGroup.Interval.Contains(age))
                                        {
                                            toy.AgeGroups.Add(ageGroup);
                                            break;
                                        }
                                    };
                                }
                            }
                        }
                    }
                    // TODO få random ud fra Category - mindre vigtigt
                    toy.Price = random.Next(49, 899);
                    toy.Stock = random.Next(1, 25);

                    if (!hasAgeGroup)
                    {
                        // sæt til alle aldersgrupper
                        toy.AgeGroups = ageGroups;
                    }
                    // TODO slet denne - det er ikke denne som skal lægge den ned i db
                    toy = Task.Run(async () => await _toyDbService.Create(toy)).Result;
                }
            }
            return toy;
        }


        public async Task<SubCategory> GetOrCreateSubCategory(string id, string name, List<Category> categories)
        {
            // tjekker om subcat allerede findes ud fra id
            SubCategory subCategory = await _subCategoryDbService.GetById(id);
            if (subCategory == null) // hvis nej
            {
                //opretter nyt obj
                subCategory = new SubCategory(id, name);

                //og tjekker hvilke over-kategorier som denne subcategory skal være indenunder
                foreach (Category category in categories)
                {
                    foreach (string sortingKeyword in category.SortingKeywords)
                    {
                        if (name.Contains(sortingKeyword))
                        {
                            // og tilføjet categorier til dens liste, så de får en relation
                            subCategory.Categories.Add(category);
                            break;
                        }
                    }
                }
                // hvis der ikke er blevet tilføjet nogle ud fra sortingkeywords, så tilføj til assorteret
                if (subCategory.Categories.Count < 1)
                {
                    foreach (Category category in categories)
                    {
                        if (category.Name.Equals("Assorteret"))
                        {
                            subCategory.Categories.Add(category);
                        }
                    }
                }
            }
            return subCategory;

        }

        public Colour GetOrCreateColour(string name)
        {
            //tjek om den er i db
            var colour = Task.Run(async () => await _colourDbService.GetByName(name)).Result;

            if (colour == null)
            {
                // opret nyt colour-obj
                colour = new Colour(name);
            }
            return colour;
        }


    }
}
