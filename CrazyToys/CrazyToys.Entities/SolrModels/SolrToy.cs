using CrazyToys.Entities.Entities;
using SolrNet.Attributes;


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
        //[SolrField("brand")]
        //public Brand Brand { get; set; }
        [SolrField("brandId")]
        public string BrandId { get; set; } // foreign key
        [SolrField("shortDescription")]
        public string ShortDescription { get; set; }
        [SolrField("longDescription")]
        public string LongDescription { get; set; }
        //[SolrField("colours")]
        //public IList<Colour> Colours { get; set; }
        //[SolrField("ageGroups")]
        //public IList<AgeGroup> AgeGroups { get; set; }
        //[SolrField("images")]
        //public IList<Image> Images { get; set; }
        //[SolrField("subCategory")]
        //public SubCategory SubCategory { get; set; } // nav-prop
        [SolrField("subCategoryId")]
        public string SubCategoryId { get; set; }// foreign key
        [SolrField("price")]
        public int Price { get; set; }
        [SolrField("stock")]
        public int Stock { get; set; }

        public SolrToy(Toy toy)
        {
            Id = toy.ID;
            ProductId = toy.ProductId;
            Name = toy.Name;
            BrandId = toy.BrandId;
            ShortDescription = toy.ShortDescription;
            LongDescription = toy.LongDescription;
            SubCategoryId = toy.SubCategoryId;
            Price = toy.Price;
            Stock = toy.Stock;
        }
    }
}
