using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.EntityDbServices;
using CrazyToys.Web.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private readonly ToyDbService _toyDbService;
        private readonly SimpleToyDbService _simpleToyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private Random random;
        private IList<AgeGroup> ageGroups;
        private IList<Category> categories;


        public IcecatDataService(IHttpClientFactory httpClientFactory, IEntityCRUD<Brand> brandDbService, IEntityCRUD<Category> categoryDbService,
            IEntityCRUD<SubCategory> subCategoryDbService, IEntityCRUD<Colour> colourDbService, ToyDbService toyDbService, SimpleToyDbService simpleToyDbService,
            IEntityCRUD<AgeGroup> ageGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _subCategoryDbService = subCategoryDbService;
            _colourDbService = colourDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
            _simpleToyDbService = simpleToyDbService;
            random = new Random();

            var ageGroupTask = _ageGroupDbService.GetAll();
            ageGroupTask.Wait();
            ageGroups = ageGroupTask.Result;

            var categoryTask = _categoryDbService.GetAll();
            categoryTask.Wait();
            categories = categoryTask.Result;
        }

        public string GetIcecatCredentials()
        {
            // TODO implementer lige noget sikkerhedsnoget her
            string username = "alphaslo";
            string password = "KJ6j1c9y8c2YwMq8GTjc";

            byte[] byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            return Convert.ToBase64String(byteArray);
        }
        
        public async Task<SimpleToy> CreateSimpleToyInDb(SimpleToy simpleToy)
        {
            return await _simpleToyDbService.Create(simpleToy);
        }

        public async Task<SimpleToy> CreateOrUpdateSimpleToyInDb(SimpleToy simpleToy)
        {
            return await _simpleToyDbService.Update(simpleToy);
        }

        public async Task<Dictionary<string, Brand>> GetBrandDict()
        {
            List<Brand> brandList = await _brandDbService.GetAll();

            return brandList.ToDictionary(keySelector: b => b.ID, elementSelector: b => b);
        }
       
        public async Task<Toy> GetSingleProduct(SimpleToy simpleToy)
        {
            Toy toy = null;

            // TODO tilføj sikkerhed ift. brugernavn
            string username = "alphaslo";
            string credentials = GetIcecatCredentials();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://live.icecat.biz/api/?UserName={username}&Language=dk&icecat_id={simpleToy.IcecatId}")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                    { HeaderNames.Authorization, $"Basic {credentials}" }
                }
            };

            HttpClient httpClient = _httpClientFactory.CreateClient();

            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    toy = new Toy();
                    toy.SimpleToyId = simpleToy.ID;
                    //toy.OnMarket = simpleToy.OnMarket.Equals("0") ? false : true;
                    bool hasAgeGroup = false; // bruges til at sætte til "Ingens Aldersgruppe"-agegroupen, hvis ingen alder-presentationvalue fundet

                    string jsonContent =
                        await httpResponseMessage.Content.ReadAsStringAsync();

                    dynamic json = JsonConvert.DeserializeObject(jsonContent);

                    toy.Name = json["data"]["GeneralInfo"]["Title"];
                    //toy.BrandId = simpleToy.SupplierId;
                    toy.BrandId = json["data"]["GeneralInfo"]["BrandID"];
                    //toy.ProductId = simpleToy.ProductId;
                    toy.ProductId = json["data"]["GeneralInfo"]["BrandPartCode"];
                    toy.ShortDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["ShortSummaryDescription"];
                    toy.LongDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["LongSummaryDescription"];

                    string subCategoryId = json["data"]["GeneralInfo"]["Category"]["CategoryID"];
                    string subCategoryName = json["data"]["GeneralInfo"]["Category"]["Name"]["Value"];

                    toy.SubCategoryId = subCategoryId;
                    SubCategory subCat = await GetOrCreateSubCategory(subCategoryId, subCategoryName, categories);

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
                                foreach (string colourName in colours)
                                {
                                    toy.Colours.Add(await GetOrCreateColour(colourName));
                                }

                            }
                            else if (featureId.Equals(ageGroupYearsId) || featureId.Equals(ageGroupMonthsId))
                            {
                                hasAgeGroup = true;
                                string presentationValue = feature["PresentationValue"];
                                toy.AgeGroup = presentationValue;

                                // uanset om det er måned eller år, så er det efter kommaet ligemeget, fordi udregningen bliver det samme
                                string age = presentationValue.Split(" ")[0].Replace(",", ".").Split(".")[0];
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
                }
                
            }

            catch (Exception e)
            {
                // TODO lav noget med at fejlen tilføjes til en log-fil eller noget - eller gør hangfire det for os? 
                Console.WriteLine("ERROR in GetSingleProduct() in IcecatDataService: " + e + "\n" + e.Message);

                LogGenerator logGenerator = new LogGenerator();
                await logGenerator.WriteExceptionToLog("IcecatDataService", "GetSingleProduct", e);

                // gem fejl i db
                simpleToy.SuccessfullyRetrievedAsJson = false;
                simpleToy.Errors.Add(new Error(e.ToString(), DateTime.Now.ToString()));
                simpleToy = await _simpleToyDbService.Update(simpleToy);

                return null;
            }
            return toy;
        }

        public async Task<Toy> CreateToyInDb(Toy toy)
        {
            return await _toyDbService.Create(toy);
        }

        public async Task<Toy> CreateOrUpdateToyInDb(Toy toy)
        {
            Toy toyFromDb = await _toyDbService.GetByProductIdAndBrandId(toy.ProductId, toy.BrandId);

            if (toyFromDb != null)
            {
                // hvis der er nogle colours på nyt toy-obj
                // Efter denne if er kørt er der altså KUN nye farver på toy'et, som IKKE allerede er tilkoblet toyFromDb
                if (toy.Colours.Count > 0)
                {
                    // hent alle farver som toyFromDb har
                    List<Colour> colours = await _toyDbService.GetColours(toyFromDb.ID);

                    //sammenlign farver på nyt obj, med de farver som allerede er tilknyttet toyFromDb
                    for (int i = toy.Colours.Count - 1; i >= 0; i--)
                    {
                        bool colourAlreadyAdded = false;
                        // vi tjekker om farven allerede er tilkoblet
                        foreach (Colour toyFromDbColour in colours)
                        {
                            if (toyFromDbColour.Name.Equals(toy.Colours[i].Name))
                            {
                                colourAlreadyAdded = true;
                                break;
                            }
                        }
                        // hvis den allerede er på: slet farven fra toy der lægges ned, så den ikke prøver at oprette farven igen
                        if (colourAlreadyAdded)
                        {
                            toy.Colours.Remove(toy.Colours[i]);
                        }
                    }
                }

                // hvis der er nogle ageGroups på nyt toy-obj
                // Efter denne if er kørt er der altså KUN nye alders grupper på toy'et, som IKKE allerede er tilkoblet toyFromDb
                if (toy.AgeGroups.Count > 0)
                {
                    // hent alle ageGroups som toyFromDb har
                    List<AgeGroup> ageGroups = await _toyDbService.GetAgeGroups(toyFromDb.ID);

                    //sammenlign ageGroups på nyt obj, med de ageGroups som allerede er tilknyttet toyFromDb
                    for (int i = toy.AgeGroups.Count - 1; i >= 0; i--)
                    {
                        bool ageGroupAlreadyAdded = false;
                        // vi tjekker om ageGroup allerede er tilkoblet
                        foreach (AgeGroup toyFromDbAgeGroup in ageGroups)
                        {
                            if (toyFromDbAgeGroup.ID.Equals(toy.AgeGroups[i].ID))
                            {
                                ageGroupAlreadyAdded = true;
                                break;
                            }
                        }
                        // hvis den allerede er på: slet alders gruppen
                        if (ageGroupAlreadyAdded)
                        {
                            toy.AgeGroups.Remove(toy.AgeGroups[i]);
                        }
                    }
                }
                toyFromDb.UpdateValuesToAnotherToysValues(toy);
                toyFromDb.Stock = 20;

                return await _toyDbService.Update(toyFromDb);
            }
            else
            {
                return await _toyDbService.Create(toy);
            }
        }

        public async Task<SubCategory> GetOrCreateSubCategory(string id, string name, IList<Category> categories)
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
                        if (name.ToLower().Contains(sortingKeyword))
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
                // Tilføj til db
                subCategory =  await _subCategoryDbService.Create(subCategory);
            }
            return subCategory;
        }

        public async Task<Colour> GetOrCreateColour(string name)
        {
            //tjek om den er i db
            var colour = await _colourDbService.GetByName(name);

            if (colour == null)
            {
                // opret nyt colour-obj
                colour = new Colour(name);
            }
            return colour;
        }

        public HashSet<SimpleToy> GetAllSimpleToysAsHashSet()
        {
            return _simpleToyDbService.GetAllAsHashSet();
        }

        public HashSet<SimpleToy> GetAllSimpleToysByDate(string dateStríng)
        {
            return _simpleToyDbService.GetAllByDate(dateStríng);
        }

        public async Task<SimpleToy> UpdateSimpleToyInDb(SimpleToy simpleToy)
        {
            return await _simpleToyDbService.Update(simpleToy);
        }

        
    }
}
