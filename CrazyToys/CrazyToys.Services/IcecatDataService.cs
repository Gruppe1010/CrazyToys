using CrazyToys.Entities.Models.Entities;
using CrazyToys.Interfaces;
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
        private Dictionary<string, Brand> brandDict;



        public IcecatDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
        }


        // 
        public async Task<Toy> getSingleProduct(string brandId, string productId)
        {
            string username = "alphaslo";
            string password = "KJ6j1c9y8c2YwMq8GTjc";

            byte[] byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            string credentials = Convert.ToBase64String(byteArray);

            string brandName;
            Brand brand;
            // TODO hent brand op fra db - gem på nyt Toy-obj
            bool hasValue = brandDict.TryGetValue(brandId, out brand);
            if (hasValue)
            {
                brandName = brand.Name;
                Console.WriteLine(brandName);
            }
            else
            {
                //TODO tjek om denne ødelægger program
                throw new KeyNotFoundException();
            }


            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://live.icecat.biz/api/?Username={username}&Language=dk&Brand={brandName}&ProductCode={productId}")
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
                string jsonContent =
                    await httpResponseMessage.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(jsonContent);

                string id = json["data"]["GeneralInfo"]["BrandPartCode"];
                string name = json["data"]["GeneralInfo"]["Title"];
                string 
                string shortDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["ShortSummaryDescription"];
                string longDescription = json["data"]["GeneralInfo"]["SummaryDescription"]["LongSummaryDescription"];
                string subCategoryId = json["data"]["GeneralInfo"]["Category"]["CategoryID"];

                // lav tjek på om subCatId allerede er i db - hvis 
                    // vi var i gang med at snakke om om den ville lave en fejl hvis vi siger add(subcat) og den allerede findes
                    // TODO tjek om den så overskriver den eller om den laver en fejl - hvis den laver en - 
                    // dbService.SubCategoryExists()


                // colour
                // stringen skal splittes op i strings og så tilføjes som seperate værdier i colour tabellen, som så skal tilføjes som refs til toy
                string colourString = json["data"]["GeneralInfo"]["FeatureGroups"]["Features"]["PresentationValue"];

                // agegroups hardcodes i db - lav noget tjek fordi den skal tildeles rigtigt



                // TODO noget med pris og stock





                // TODO gør noget med colour



            }

            return new Toy();
            throw new NotImplementedException();
        }
        
    }
}
