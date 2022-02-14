using CrazyToys.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class HangfireService : IHangfireService
    {
        private readonly IProductDataService _icecatDataService;

        private HangfireService(IProductDataService icecatDataService)
        {
            _icecatDataService = icecatDataService;
        }

        public void GetDaily()
        {
            throw new NotImplementedException();
        }

        public void GetIndex()
        {

            _toyContext.Brands.Add(new Brand("1", "gruppe10"));
            await _toyContext.SaveChangesAsync();


            string username = "alphaslo";
            string password = "KJ6j1c9y8c2YwMq8GTjc";

            var byteArray = Encoding.ASCII.GetBytes($"{username}:{password}");
            string credentials = Convert.ToBase64String(byteArray);


            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                "https://data.Icecat.biz/export/freexml/EN/daily.index.xml")
            {
                Headers =
                    {
                        { HeaderNames.Accept, "application/xml" },
                        { HeaderNames.Authorization, $"Basic {credentials} " }
                    }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                /* // Dat vi arbejde med string og lavede det om til json:
                var noget  = contentString.Split("<files.index Generated=\"20220201050001\">"); // TODO denne er hardcodet - den er en anden i morgeeeen
                var noget1 = noget[1].Split("</files.index>");
                contentString = noget1[0];

                contentString = "<files>" + contentString + "</files>";


                XmlDocument doc = new XmlDocument();
                doc.LoadXml(contentString);

                string jsonString = JsonConvert.SerializeXmlNode(doc);

                JObject json = JObject.Parse(jsonString);

                IEnumerable<JProperty> generated = json.Properties();
                */

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Async = true;
                settings.IgnoreWhitespace = true;
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.IgnoreComments = true;

                using (XmlReader reader = XmlReader.Create(contentStream, settings))
                {

                    while (await reader.ReadAsync())
                    {
                        switch (reader.Name)//(reader.NodeType)
                        {
                            case "file": //XmlNodeType.Element: // Her ved vi at det er en file

                                if (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    string supplierId = reader.GetAttribute("Supplier_id");
                                    Console.WriteLine("supplierId: " + supplierId);

                                    if (brandDict.ContainsKey(supplierId))
                                    {
                                        string productId = reader.GetAttribute("Prod_ID");

                                        //brandDict[supplierId].ProductIdSet.Add(productId);


                                    }
                                }


                                break;
                            default:
                                break;
                        }
                    }
                }


            }
        }
    }
}
