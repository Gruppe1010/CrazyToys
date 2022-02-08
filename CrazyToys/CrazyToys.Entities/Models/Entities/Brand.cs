using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Models.Entities
{
    public class Brand
    {
        [Column("BrandId")]
        public string ID { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }

        public Brand(string iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public Brand(string iD, string name, string logoUrl) : this(iD, name)
        {
            LogoUrl = logoUrl;
        }
    }
}