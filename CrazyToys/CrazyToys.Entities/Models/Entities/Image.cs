using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Models.Entities
{
    public class Image
    {

        [Column("ImageId")]
        public string ID { get; set; }
        public string UrlLow { get; set; }
        public string UrlHigh { get; set; }

        public Image(string urlLow, string urlHigh)
        {
            UrlLow = urlLow;
            UrlHigh = urlHigh;
        }
    }
}