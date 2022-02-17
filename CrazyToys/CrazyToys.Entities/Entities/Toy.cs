using CrazyToys.Entities.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Toy
    {
        [Column("ToyId")]
        public string ID { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public string AgeGroup { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public ICollection<Colour> Colours { get; set; }
        public ICollection<AgeGroup> AgeGroups { get; set; }
        public SubCategory SubCategory { get; set; }
        public ICollection<Image> Images { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public bool OnMarket { get; set; }

        public Toy()
        {
            Colours = new List<Colour>();
            AgeGroups = new List<AgeGroup>();
            Images = new List<Image>();
        }

        public Toy(string iD, string name, Brand brand, string ageGroup, string shortDescription,
            string longDescription, ICollection<Colour> colours, ICollection<AgeGroup> ageGroups,
            SubCategory subCategory, int price, int stock, ICollection<Image> images, bool onMarket)
        {
            ID = iD;
            Name = name;
            Brand = brand;
            AgeGroup = ageGroup;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            Colours = colours;
            AgeGroups = ageGroups;
            SubCategory = subCategory;
            Price = price;
            Stock = stock;
            Images = images;
            OnMarket = onMarket;
        }

        public ToyDTO ConvertToDTO()
        {
            // TODO tag AgeGroups og SubCategories med - opret convertere til dem og find ud af hvordan man bruger Map - er det fordi det er en ICollection?
            return new ToyDTO(ID, Name, Brand, AgeGroup, ShortDescription, LongDescription, Price, Stock, Images);
        }

        public void UpdateValuesToAnotherToysValues(Toy toy)
        {
            ID = toy.ID;
            Name = toy.Name;
            Brand = toy.Brand;
            AgeGroup = toy.AgeGroup;
            ShortDescription = toy.ShortDescription;
            LongDescription = toy.LongDescription;
            SubCategory = toy.SubCategory;
            Price = toy.Price;
            Stock = toy.Stock;
            Images = toy.Images;
            OnMarket = toy.OnMarket;

            /*
            Colours = toy.Colours;
            AgeGroups = toy.AgeGroups;
            */
            
        }


    }
}