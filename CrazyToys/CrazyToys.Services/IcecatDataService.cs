using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using CrazyToys.Services.ProductDbServices;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CrazyToys.Services
{
    public class IcecatDataService : IProductDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEntityCRUD<Brand> _brandDbService;
        private readonly CategoryDbService _categoryDbService;
        private readonly IEntityCRUD<SubCategory> _subCategoryDbService;
        private readonly IEntityCRUD<ColourGroup> _colourGroupDbService;
        private readonly ToyDbService _toyDbService;
        private readonly SimpleToyDbService _simpleToyDbService;
        private readonly IEntityCRUD<AgeGroup> _ageGroupDbService;
        private readonly ImageDbService _imageDbService;
        private readonly IEntityCRUD<PriceGroup> _priceGroupDbService;

        private Random random;
        private IList<AgeGroup> ageGroups;
        private IList<PriceGroup> priceGroups;
        private IList<ColourGroup> colourGroups;
        private IList<Category> categories;
        private Dictionary<string, Brand> brandDict;



        public IcecatDataService(
            IHttpClientFactory httpClientFactory, IEntityCRUD<Brand> brandDbService,
            CategoryDbService categoryDbService,IEntityCRUD<SubCategory> subCategoryDbService, 
            IEntityCRUD<ColourGroup> colourGroupDbService, ToyDbService toyDbService, 
            SimpleToyDbService simpleToyDbService,IEntityCRUD<AgeGroup> ageGroupDbService, 
            ImageDbService imageDbService,IEntityCRUD<PriceGroup> priceGroupDbService)
        {
            _httpClientFactory = httpClientFactory;
            _brandDbService = brandDbService;
            _categoryDbService = categoryDbService;
            _subCategoryDbService = subCategoryDbService;
            _colourGroupDbService = colourGroupDbService;
            _toyDbService = toyDbService;
            _ageGroupDbService = ageGroupDbService;
            _simpleToyDbService = simpleToyDbService;
            _imageDbService = imageDbService;
            _priceGroupDbService = priceGroupDbService;
            random = new Random();

            var ageGroupTask = _ageGroupDbService.GetAll();
            ageGroupTask.Wait();
            ageGroups = ageGroupTask.Result;

            var priceGroupTask = _priceGroupDbService.GetAll();
            priceGroupTask.Wait();
            priceGroups = priceGroupTask.Result;

            var colourGroupTask = _colourGroupDbService.GetAll();
            colourGroupTask.Wait();
            colourGroups = colourGroupTask.Result;

            var categoryTask = _categoryDbService.GetAll();
            categoryTask.Wait();
            categories = categoryTask.Result;

            var brandTask = GetBrandDict();
            brandTask.Wait();
            brandDict = brandTask.Result;
        }


        /*
         * Henter enten index eller daily-fil fra icecat
         * Den tjekker alle produkter om brand-id passer på vores udvalgte brands
         *      hvis ja --> henter den værdier til at lave SimpleToy-obj, som lægges ned i db
         * Efter ALLE produkter i fil er løbet igennem og lagt i db, hentes de op og oprettes som Toy-obj
         * 
         * De lægges FØRST ned i SimpleToy-tabel for at undgå at processen af at hente alt fra filen afbrydes, hvis der sker en fejl i hentningen af et enkelt produkt
         * **/
        public async Task GetProductsFromIcecat(string url)
        {
            string credentials = GetIcecatCredentials();

            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get, url)
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/xml" },
                        { HeaderNames.Authorization, $"Basic {credentials} " }
                    }
            };

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.Timeout = TimeSpan.FromMinutes(1000);
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Dictionary<string, Brand> brandDict = await GetBrandDict();
                string dateString = DateTime.Now.ToString();

                var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;
                settings.IgnoreWhitespace = true;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.IgnoreComments = true;

                XmlReader reader = XmlReader.Create(contentStream, settings);

                while (await reader.ReadAsync())
                {
                    if (reader.Name == "file" && (reader.NodeType != XmlNodeType.EndElement))
                    {
                        string supplierId = reader.GetAttribute("Supplier_id");

                        if (brandDict.ContainsKey(supplierId))
                        {
                            //string productId = reader.GetAttribute("Prod_ID");
                            string onMarket = reader.GetAttribute("On_Market");
                            string icecatId = reader.GetAttribute("Product_ID");

                            SimpleToy simpleToy = url.Contains("daily")
                                ? await CreateOrUpdateSimpleToyInDb(new SimpleToy(supplierId, onMarket, icecatId, dateString))
                                : await CreateSimpleToyInDb(new SimpleToy(supplierId, onMarket, icecatId, dateString));
                        }
                    }
                }
                // nu har vi lagt alle toys fra enten index eller daily successfuldt - nu skal de hentes op og lægges ned som Toy-objs
                await CreateToysFromSimpleToys(url.Contains("daily"), dateString);
            }
        }
        /*
        * Denne metode henter alle SimpleToys op, som lige er blevet lagt i db ud fra index eller daily
        * Derefter henter den den fulde produktinfo i json, laver et nyt toy-obj og tilføjer til db 
        * - hvis det er daily laver den CreateOrUpdate 
        * - hvis det er index findes der ikke nogen toy-obj i db, og derfor laver den bare Create()
        ***/
        public async Task CreateToysFromSimpleToys(bool isDaily, string dateString)
        {
            HashSet<SimpleToy> simpleToys = isDaily
                ? GetAllSimpleToysByDate(dateString)
                : GetAllSimpleToysAsHashSet();

            foreach (SimpleToy simpleToy in simpleToys)
            {
                //if (!simpleToy.ProductId.Contains("E+25"))
                Toy toy = await GetSingleProduct(simpleToy);
                if (toy != null)
                {
                    Toy addedToy = isDaily
                                  ? await CreateOrUpdateToyInDb(toy)
                                  : await CreateToyInDb(toy); // TODO det ville så være her at man så skulle tilføje ting som fx forbindelse til colourgroups og pricegroups osv.
                }
            }
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
            return await _simpleToyDbService.GetByProductIcecatId(simpleToy.IcecatId) == null
                ? await _simpleToyDbService.Create(simpleToy)
                : simpleToy;
        }

        public async Task<SimpleToy> CreateOrUpdateSimpleToyInDb(SimpleToy simpleToy)
        {
            SimpleToy simpleToyFromDb = await _simpleToyDbService.GetByProductIcecatId(simpleToy.IcecatId);

            if (simpleToyFromDb != null)
            {
                simpleToyFromDb.UpdateValuesToAnotherToysValues(simpleToy);
                return await _simpleToyDbService.Update(simpleToyFromDb);
            }
            return await _simpleToyDbService.Create(simpleToy);
        }

        public async Task<Dictionary<string, Brand>> GetBrandDict()
        {
            List<Brand> brandList = await _brandDbService.GetAll();

            return brandList.ToDictionary(keySelector: b => b.ID, elementSelector: b => b);
        }

        /*
         TODO overvej om vi skal omstrukturere metoden, så den først bare henter dataen ud som vi får og gemmer et toy 
        - og så bagefter kan vi tjekke på fx subcat og colour om der er ændringer - for hvis der ikke er behøver man jo ikke køre hele baduljen af koden igennem
         */
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
                    bool hasAgeGroup = false; // bruges til at sætte til "Ingens Aldersgruppe"-agegroupen, hvis ingen alder-presentationvalue fundet

                    string jsonContent =
                        await httpResponseMessage.Content.ReadAsStringAsync();

                    //dynamic json = JsonConvert.DeserializeObject(jsonContent);

                    dynamic json = JObject.Parse(jsonContent);


                    toy.BrandId = json.data.GeneralInfo.BrandID;//["data"]["GeneralInfo"]["BrandID"];
                    toy.ProductId = json.data.GeneralInfo.BrandPartCode;
                    toy.ShortDescription = json.data.GeneralInfo.SummaryDescription.ShortSummaryDescription;
                    toy.LongDescription = json.data.GeneralInfo.SummaryDescription.LongSummaryDescription;
                    string name = json.data.GeneralInfo.Title;

                    name = name
                        .Replace(toy.ProductId, "")
                        .Trim();

                    toy.Name = char.ToUpper(name[0]) + name.Substring(1);

                    string subCategoryId = json.data.GeneralInfo.Category.CategoryID;
                    string subCategoryName = json.data.GeneralInfo.Category.Name.Value;

                    toy.SubCategoryId = subCategoryId;
                    SubCategory subCat = await GetOrCreateSubCategory(subCategoryId, subCategoryName, categories);
                    toy.SubCategory = subCat;

                    string urlHigh = json.data.Image.HighPic;

                    Image image = new Image(urlHigh, 0);
                    toy.Images.Add(image);

                    // tilføj resten af billeder som ligger i Gallery-key
                    foreach (dynamic img in json.data.Gallery)
                    {
                        string galleryImageUrlHigh = img.Pic;
                        int galleryImageNo = img.No;

                        Image galleryImage = new Image(galleryImageUrlHigh, galleryImageNo);
                        toy.Images.Add(galleryImage);
                    }

                    // FeaturesGroups --> for hver på listen: ["Features"] for hver på listen: ["Feature"] if ["id"] = 1766 -->  item på ["Features"]["PresentationValue"]
                    // stringen skal splittes op i strings og så tilføjes som seperate værdier i colour tabellen, som så skal tilføjes som refs til toy
                    //string colourString = json["data"]["GeneralInfo"]["FeaturesGroups"]["Features"]["PresentationValue"];
                    string colourPresentationValueId = "1766"; 
                    string ageGroupYearsPresentationValueId = "24697";
                    string ageGroupMonthsPresentationValueId = "24019";

                    var featureGroups = json.data.FeaturesGroups;
                    foreach (dynamic featuresGroup in json.data.FeaturesGroups)
                    {
                        dynamic features = featuresGroup.Features;

                        foreach (dynamic feature in features)
                        {

                            string featureId = feature.Feature.ID;

                            if (featureId.Equals(colourPresentationValueId))
                            {
                                string presentationValue = feature.PresentationValue;

                                toy.Colours = presentationValue;

                                if (!string.IsNullOrWhiteSpace(toy.Colours))
                                {
                                    AddColourGroupsFromColour(toy);
                                }

                            }
                            else if (featureId.Equals(ageGroupYearsPresentationValueId) || featureId.Equals(ageGroupMonthsPresentationValueId))
                            {
                                hasAgeGroup = true;
                                string presentationValue = feature.PresentationValue;
                                toy.AgeGroup = presentationValue;

                                // uanset om det er måned eller år, så er det efter kommaet ligemeget, fordi udregningen bliver det samme
                                string age = presentationValue.Split(" ")[0].Replace(",", ".").Split(".")[0];
                                int ageAsInt = Convert.ToInt32(age);

                                // hvis det er i måneder, skal det konverteres til år
                                if (featureId.Equals(ageGroupMonthsPresentationValueId))
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
                    
                    int priceGroupIndex = random.Next(0, priceGroups.Count - 1);
                    int priceGroupMin = priceGroups[priceGroupIndex].Minimum;
                    int priceGroupMax = priceGroups[priceGroupIndex].Maximum;

                    toy.PriceGroup = priceGroups[priceGroupIndex];

                    int price = random.Next(
                        priceGroupMin != 0 ? priceGroupMin : 49,
                        priceGroupMax != 0 ? priceGroupMax : 900);
                    // % 10 for at få den sidste digit
                    // for at gøre sidste digit 9
                    toy.Price = price + (9 - (price % 10));
                    toy.Stock = random.Next(1, 25);

                    if (!hasAgeGroup)
                    {
                        // hvis der ikke er blevet tildelt nogen aldersgruppe, så sæt til alle aldersgrupper
                        // (fordi så er det fordi de ikke har sat nogen alder på toy'et vi får ind)
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

        public void AddColourGroupsFromColour(Toy toy)
        {
            string toyColour = toy.Colours.ToLower();
            foreach(ColourGroup colourGroup in colourGroups)
            {
                foreach(string sortingKeyWord in colourGroup.SortingKeywords)
                {
                    if (toyColour.Contains(sortingKeyWord))
                    {
                        toy.ColourGroups.Add(colourGroup);
                        break; // så går den videre til at tjekke næste colourGroup
                    }
                }
            }
        }

        public async Task<Toy> CreateToyInDb(Toy toy)
        {
            return await _toyDbService.GetByProductIdAndBrandId(toy.ProductId, toy.BrandId) == null
                ? await _toyDbService.Create(toy)
                : toy;
        }

        public async Task<Toy> CreateOrUpdateToyInDb(Toy toy)
        {
            // TODO tilføj at den også henter Colour
            Toy toyFromDb = await _toyDbService.GetByProductIdAndBrandId(toy.ProductId, toy.BrandId);

            if (toyFromDb != null)
            {
                // Alt dette er slet ikke nødvendigt - det virker uden!
                await RemoveDuplicateAgeGroups(toyFromDb, toy);

                toyFromDb.UpdateValuesToAnotherToysValues(toy);
                return await _toyDbService.Update(toyFromDb);
            }
            else
            {
                return await _toyDbService.Create(toy);
            }
        }

        // TODO slet! Passer dette stadig? 
        public async Task RemoveDuplicateAgeGroups(Toy toyFromDb, Toy toy)
        {
          
            if(toyFromDb.Images.Count > 0)
            {
                // slet fra db
                await _imageDbService.DeleteRange(toyFromDb.Images);
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
        }

        public async Task<SubCategory> GetOrCreateSubCategory(string id, string name, IList<Category> categories)
        {
            // tjekker om subcat allerede findes ud fra id
            SubCategory subCategory = await _subCategoryDbService.GetById(id);
            if (subCategory == null) // hvis nej
            {
                // fjerner & i subcats navn
                if (name.Contains("&"))
                {
                    name = name.Replace("&", "og");
                }

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
            }
            return subCategory;
        }

        public async Task<ColourGroup> GetOrCreateColour(string name)
        {
            //tjek om den er i db

            var colour = await _colourGroupDbService.GetByName(name);

            if (colour == null)
            {
                // opret nyt colour-obj
                colour = new ColourGroup(name);
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
