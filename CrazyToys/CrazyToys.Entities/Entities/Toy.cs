using CrazyToys.Entities.DTOs;
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
        public PriceGroup PriceGroup { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string Colours { get; set; }
        public IList<ColourGroup> ColourGroups { get; set; }
        public IList<AgeGroup> AgeGroups { get; set; }
        public IList<Image> Images { get; set; }
        public SubCategory SubCategory { get; set; } // nav-prop
        public string SubCategoryId { get; set; }// foreign key
        public SimpleToy SimpleToy { get; set; } // nav-prop
        public string SimpleToyId { get; set; }// foreign key
        public int Price { get; set; }
        public int Stock { get; set; }

        public Toy()
        {
            ColourGroups = new List<ColourGroup>();
            AgeGroups = new List<AgeGroup>();
            Images = new List<Image>();
        }


        public void UpdateValuesToAnotherToysValues(Toy toy)
        {
            ProductId = toy.ProductId;
            Name = toy.Name;
            BrandId = toy.BrandId;
            AgeGroup = toy.AgeGroup;
            PriceGroup = toy.PriceGroup;
            ShortDescription = toy.ShortDescription;
            LongDescription = toy.LongDescription;
            SubCategoryId = toy.SubCategoryId;
            SimpleToyId = toy.SimpleToyId;
            Price = toy.Price;
            Stock = toy.Stock;
            Images = toy.Images;
            Colours = toy.Colours;
            ColourGroups = toy.ColourGroups;
            AgeGroups = toy.AgeGroups;
        }


        public ShoppingCartToyDTO ConvertToShoppingCartToyDTO(int quantity)
        {
            return new ShoppingCartToyDTO(ID, Name, Price, quantity, Images.Count > 0 ? Images[0].UrlHigh : null, Stock);
        }

        /*
        public ShopToyDTO ConvertToShopToyDTO()
        {
            return new ShopToyDTO(ID, Name, Price, Images.Count > 0 ? Images[0].UrlHigh : null);
        }
        */

        public WishlistToyDTO ConvertToWishlistToyDTO()
        {
            return new WishlistToyDTO(ID, Name, Price, Images.Count > 0 ? Images[0].UrlHigh : null, Stock);
        }

    }
}