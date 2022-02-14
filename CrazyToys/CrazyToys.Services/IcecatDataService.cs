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
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;



        //private Dictionary<string, Brand> brandDict;
        private Random random;



        public IcecatDataService(IHttpClientFactory httpClientFactory, BrandDbService brandDbService, CategoryDbService categoryDbService, 
            SubCategoryDbService subCategoryDbService, ColourDbService colourDbService, AgeGroupDbService ageGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _subCategoryDbService = subCategoryDbService;
            _colourDbService = colourDbService;
            _ageGroupDbService = ageGroupDbService;
            random = new Random();

            /*
            brandDict = new Dictionary<string, Brand>();

            brandDict.Add("15111", new Brand("15111", "Barbie", "https://images.icecat.biz/img/brand/thumb/15111_e16ba4de456d43bd993b5c39607aa845.jpg"));
            brandDict.Add("5669", new Brand("5669", "Hasbro", "https://images.icecat.biz/img/brand/thumb/5669_5c735b62136a4d32b553ed74c66cdb15.jpg"));
            brandDict.Add("27814", new Brand("27814", "Bambolino", "https://images.icecat.biz/img/brand/thumb/27814_29d6d8a1fda04558a4cae1a0b9a7d175.jpg"));
            brandDict.Add("23505", new Brand("23505", "Clown", "https://images.icecat.biz/img/brand/thumb/23505_3387646289f8463ebca32c8b5fded65b.jpg"));
            brandDict.Add("32046", new Brand("32046", "Fuzzikins Fuzzi", "https://images.icecat.biz/img/brand/thumb/32046_d6cefd82d5714ed08af17b4eea0f3237.jpg"));
            brandDict.Add("7375", new Brand("7375", "IMC Toys", "https://images.icecat.biz/img/brand/thumb/7375_62ea0b83ce6e454daf6b5a5910c53d87.jpg"));
            brandDict.Add("24094", new Brand("24094", "LumoStars", "https://images.icecat.biz/img/brand/thumb/24094_277b2a8a5f75443ababf44ae56acf1cd.jpg"));
            brandDict.Add("36068", new Brand("36068", "My Little Pony", "https://images.icecat.biz/img/brand/thumb/36068_33f45a22fbb042ff979cc93c18f17c00.jpg"));
            brandDict.Add("16933", new Brand("16933", "Play-Doh", "https://images.icecat.biz/img/brand/thumb/16933_fd0ecf980706469dbc94333be9e1e435.jpg"));
            brandDict.Add("15136", new Brand("15136", "Polly Pocket", "https://images.icecat.biz/img/brand/thumb/15136_55e19fa2894d4273810836c223673f4c.jpg"));
            brandDict.Add("16046", new Brand("16046", "SESCreative", "https://images.icecat.biz/img/brand/thumb/16046_dd661f6b89de45819bb760a18e9c81ef.jpg"));
            brandDict.Add("23442", new Brand("23442", "RuboToys", "https://images.icecat.biz/img/brand/thumb/23442_16e7f8ad9bde491c8619a1d68d09c25a.jpg"));
            brandDict.Add("3480", new Brand("3480", "Jumbo", "https://images.icecat.biz/img/brand/thumb/3480_998702f9778f4b2cad3b2b69d98ee481.jpg"));
            */
        }


        // 
        public async Task<Toy> GetSingleProduct(string brandId, string productId)
        {
            Toy toy = null;

            string username = "alphaslo";
            string password = "KJ6j1c9y8c2YwMq8GTjc";

            byte[] byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            string credentials = Convert.ToBase64String(byteArray);

            
            Brand brand = await _brandDbService.GetById(brandId);
            if (brand != null)
            {
                bool hasAgeGroup = false;
                var ageGroups = Task.Run(async () => await _ageGroupDbService.GetAll()).Result;
                var categories = Task.Run(async () => await _categoryDbService.GetAll()).Result;


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

                    string jsonContent =
                        await httpResponseMessage.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(jsonContent);


                    string id = json["data"]["GeneralInfo"]["BrandPartCode"];


                    toy.ID = json["data"]["GeneralInfo"]["BrandPartCode"];
                    toy.Name = json["data"]["GeneralInfo"]["Title"];

                    toy.ShortDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["ShortSummaryDescription"];
                    toy.LongDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["LongSummaryDescription"];


                    // TODO gør noget med subcat
                    string subCategoryId = json["data"]["GeneralInfo"]["Category"]["CategoryID"];

                    // lav tjek på om subCatId allerede er i db - hvis 
                    // vi var i gang med at snakke om om den ville lave en fejl hvis vi siger add(subcat) og den allerede findes
                    // TODO tjek om den så overskriver den eller om den laver en fejl - hvis den laver en - 
                    // dbService.SubCategoryExists()
                    SubCategory subCategory = await _subCategoryDbService.GetById(subCategoryId);
                    if(subCategory == null)
                    {
                        // tilføj ny subcat til db
                        string subCategoryName = json["data"]["GeneralInfo"]["Category"]["Name"]["Value"];

                        // TODO hent Category-obj op som subCat skal være inde under





                        subCategory = new SubCategory(subCategoryId, subCategoryName);

                        // gem ned i db
                        await _subCategoryDbService.Create(subCategory);

                    }
                    toy.SubCategory = subCategory;

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

                    // colour - 1766 
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

                                // for hver farve
                                Array.ForEach(colours, colourName =>
                                {
                                    //tjek om den er i db
                                    var colour = Task.Run(async () => await _colourDbService.GetByName(colourName)).Result;
                                                                     
                                    if(colour == null)
                                    {
                                        // ellers læg i db
                                        colour = Task.Run(async () => await _colourDbService.Create(new Colour(colourName))).Result;
                                    } 
                                    //og tilføj farven til toy-obj
                                    toy.Colours.Add(colour);
                                });
                            }
                            else if (featureId.Equals(ageGroupYearsId) || featureId.Equals(ageGroupMonthsId))
                            {
                                hasAgeGroup = true;
                                string presentationValue = feature["PresentationValue"];
                                toy.AgeGroup = presentationValue;

                                // uanset om det er måned eller år, så er det efter kommaet ligemeget, fordi udregningen bliver det samme
                                string age = presentationValue.Split(" ")[0].Split(".")[0];
                                int ageAsInt = Convert.ToInt32(age);


                                if (featureId.Equals(ageGroupMonthsId))
                                {
                                    ageAsInt = ageAsInt / 12;
                                }
                                if (ageAsInt == 0)
                                {
                                    foreach (AgeGroup ageGroup in ageGroups)
                                    {
                                        if (ageGroup.Interval.Contains("0"))
                                        {
                                            toy.AgeGroups.Add(ageGroup);
                                            break;
                                        }
                                    };
                                }
                                else if (ageAsInt > 8)
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
                                else
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
                    toy.Price = random.Next(0, 899);
                    toy.Stock = random.Next(0, 10);
                }

                if (!hasAgeGroup)
                {
                    // sæt til alle aldersgrupper
                    toy.AgeGroups = ageGroups;
                }

            }
            return toy;
        }
    }
}
