using CrazyToys.Entities.Entities;
using SolrNet.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace CrazyToys.Entities.SolrModels
{
    public class SolrToy
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }
        [SolrField("productId")]
        public string ProductId { get; set; }
        [SolrField("name")]
        public string Name { get; set; }
        [SolrField("brand")]
        public string Brand { get; set; }
        [SolrField("colour")]
        public string Colour { get; set; }
        [SolrField("colourGroups")]
        public List<string> ColourGroups { get; set; }
        [SolrField("ageGroup")]
        public string AgeGroup { get; set; }
        [SolrField("ageGroupIntervals")]
        public IList<string> AgeGroupIntevals { get; set; }
        [SolrField("image")]
        public string Image { get; set; }
        [SolrField("categories")]
        public IList<string> Categories { get; set; }
        [SolrField("subCategory")]
        public string SubCategory { get; set; }
        [SolrField("price")]
        public int Price { get; set; }

        public SolrToy(Toy toy)
        {
            Id = toy.ID;
            ProductId = toy.ProductId;
            Name = toy.Name;
            Categories = toy.SubCategory.Categories.Select(c => c.Name).ToList();
            Price = toy.Price;
            Brand = toy.Brand.Name;
            Colour = toy.Colour;
            ColourGroups = toy.ColourGroups.Select(c => c.Name).ToList();
            AgeGroup = toy.AgeGroup;
            AgeGroupIntevals = toy.AgeGroups.Select(a => a.Interval).ToList();
            // TODO ændr på imag
            Image = toy.Images[0].UrlHigh;
            SubCategory = toy.SubCategory.Name;
        }

        public SolrToy()
        {

        }

        /*
        [SolrUniqueKey("id")]
        public string Id { get; set; }
        [SolrField("productId")]
        public string ProductId { get; set; }
        [SolrField("name")]
        public IList<string> Name { get; set; }
        [SolrField("brand")]
        public string Brand { get; set; }
        [SolrField("colour")]
        public IList<string> Colour { get; set; }
        [SolrField("colourGroups")]
        public List<string> ColourGroups { get; set; }
        [SolrField("ageGroup")]
        public IList<string> AgeGroup { get; set; }
        [SolrField("ageGroupIntervals")]
        public IList<string> AgeGroupIntevals { get; set; }
        [SolrField("image")]
        public string Image { get; set; }
        [SolrField("categories")]
        public IList<string> Categories { get; set; }
        [SolrField("subCategory")]
        public IList<string> SubCategory { get; set; }
        [SolrField("price")]
        public int Price { get; set; }

        public SolrToy(Toy toy)
        {
            Id = toy.ID;
            ProductId = toy.ProductId;
            Name = (IList<string>)toy.Name.ToList();
            Categories = toy.SubCategory.Categories.Select(c => c.Name).ToList();
            Price = toy.Price;
            Brand = toy.Brand.Name;
            Colour = (IList<string>)toy.Colour.ToList();
            ColourGroups = toy.ColourGroups.Select(c => c.Name).ToList();
            AgeGroup = (IList<string>)toy.AgeGroup.ToList();
            AgeGroupIntevals = toy.AgeGroups.Select(a => a.Interval).ToList();
            // TODO ændr på imag
            Image = toy.Images[0].UrlHigh;
            SubCategory = (IList<string>)toy.SubCategory.Name.ToList();
        }
        */
    }
}
