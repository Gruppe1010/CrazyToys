using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string UrlLow { get; set; }
        public string UrlHigh { get; set; }
        public int OrderNumber { get; set; }

        public Image(string urlLow, string urlHigh, int orderNumber)
        {
            UrlLow = urlLow;
            UrlHigh = urlHigh;
            OrderNumber = orderNumber;
        }
    }
}