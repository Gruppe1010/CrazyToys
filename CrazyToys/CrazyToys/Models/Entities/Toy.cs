
using CrazyToys.Web.Models.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class Toy
    {
        [Column("ToyId")]
        public string ID { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public Colour Colour { get; set; }
        public ICollection<AgeGroup> AgeGroups { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public ICollection<Image> Images { get; set; }


        public ToyDTO ConvertToDTO()
        {
            // TODO tag AgeGroups og SubCategories med - opret convertere til dem og find ud af hvordan man bruger Map - er det fordi det er en ICollection?
            return new ToyDTO(ID, Name, Brand, ShortDescription, LongDescription, Colour, Price, Stock, Images);
        }

    }
}