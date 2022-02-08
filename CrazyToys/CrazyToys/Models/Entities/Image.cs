using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class Image
    {

        [Column("ImageId")]
        public string ID { get; set; }
        public string UrlLow { get; set; }
        public string UrlHigh { get; set; }

    }
}