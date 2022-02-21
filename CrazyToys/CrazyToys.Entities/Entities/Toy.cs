﻿using CrazyToys.Entities.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Entities.Entities
{
    public class Toy
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; } // nav-prop
        public string BrandId { get; set; } // foreign key
        public string AgeGroup { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public IList<Colour> Colours { get; set; }
        public IList<AgeGroup> AgeGroups { get; set; }
        public IList<Image> Images { get; set; }
        public SubCategory SubCategory { get; set; } // nav-prop
        public string SubCategoryId { get; set; }// foreign key
        //public SimpleToy SimpleToy { get; set; } // nav-prop
        //public string SimpleToyId { get; set; }// foreign key
        public int Price { get; set; }
        public int Stock { get; set; }
        public bool OnMarket { get; set; }

        public Toy()
        {
            Colours = new List<Colour>();
            AgeGroups = new List<AgeGroup>();
            Images = new List<Image>();
        }

        public Toy(string iD, string productId, string name, string brandId, string ageGroup, string shortDescription,
            string longDescription, List<Colour> colours, IList<AgeGroup> ageGroups,
            string subCategoryId, /*string simpleToyId,*/ int price, int stock, IList<Image> images, bool onMarket)
        {
            ID = iD;
            ProductId = productId;
            Name = name;
            BrandId = brandId;
            AgeGroup = ageGroup;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            Colours = colours;
            AgeGroups = ageGroups;
            SubCategoryId = subCategoryId;
            //SimpleToyId = simpleToyId;
            Price = price;
            Stock = stock;
            Images = images;
            OnMarket = onMarket;
        }

        public ToyDTO ConvertToDTO()
        {
            // TODO tag AgeGroups og SubCategories med - opret convertere til dem og find ud af hvordan man bruger Map - er det fordi det er en ICollection?
            return new ToyDTO(ID, ProductId, Name, Brand, AgeGroup, ShortDescription, LongDescription, Price, Stock, Images);
        }

        public void UpdateValuesToAnotherToysValues(Toy toy)
        {
            ProductId = toy.ProductId;
            Name = toy.Name;
            BrandId = toy.BrandId;
            AgeGroup = toy.AgeGroup;
            ShortDescription = toy.ShortDescription;
            LongDescription = toy.LongDescription;
            SubCategoryId = toy.SubCategoryId;
            //SimpleToyId = toy.SimpleToyId;
            Price = toy.Price;
            Stock = toy.Stock;
            Images = toy.Images;
            OnMarket = toy.OnMarket;
            Colours = toy.Colours;
            AgeGroups = toy.AgeGroups;
        }
    }
}