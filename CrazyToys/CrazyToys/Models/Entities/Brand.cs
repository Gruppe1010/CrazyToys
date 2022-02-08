using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class Brand
    {
        [Column("IdBrand")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }


    }
}