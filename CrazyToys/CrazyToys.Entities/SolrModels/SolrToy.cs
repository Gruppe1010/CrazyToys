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
        public ICollection<string> Name { get; set; }
        [SolrField("brand")]
        public string Brand { get; set; }
        [SolrField("colours")]
        public ICollection<string> Colours { get; set; }
        [SolrField("colourGroups")]
        public List<string> ColourGroups { get; set; }
        [SolrField("ageGroup")]
        public ICollection<string> AgeGroup { get; set; }
        [SolrField("ageGroupIntervals")]
        public IList<string> AgeGroupIntevals { get; set; }
        [SolrField("image")]
        public string Image { get; set; }
        [SolrField("categories")]
        public IList<string> Categories { get; set; }
        [SolrField("subCategory")]
        public ICollection<string> SubCategory { get; set; }
        [SolrField("price")]
        public int Price { get; set; }
        [SolrField("priceGroup")]
        public string PriceGroup { get; set; }
        [SolrField("soldAmount")]
        public int SoldAmount { get; set; }

        public SolrToy()
        {
        }

        public SolrToy(Toy toy)
        {
            Id = toy.ID;
            ProductId = toy.ProductId;
            Name = new List<string>();  

            Name.Add(toy.Name);
            Categories = toy.SubCategory.Categories.Select(c => c.Name).ToList();
            Price = toy.Price;
            Brand = toy.Brand.Name;
            Colours = new List<string>();

            Colours.Add(toy.Colours);
            ColourGroups = toy.ColourGroups.Select(c => c.Name).ToList();
            AgeGroup = new List<string>();

            AgeGroup.Add(toy.AgeGroup);
            AgeGroupIntevals = toy.AgeGroups.Select(a => a.Interval).ToList();
            // TODO ændr på imag
            Image = toy.Images[0].UrlHigh;

            SubCategory = new List<string>();

            SubCategory.Add(toy.SubCategory.Name);
            PriceGroup = toy.PriceGroup.Interval;
            // TODO SoldAmout = noget order.quantity
        }
    }
}
