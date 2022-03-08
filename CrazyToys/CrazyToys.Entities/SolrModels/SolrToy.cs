﻿using CrazyToys.Entities.Entities;
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
        [SolrField("shortDescription")]
        public string ShortDescription { get; set; }
        [SolrField("longDescription")]
        public string LongDescription { get; set; }
        [SolrField("colour")]
        public string Colour { get; set; }
        [SolrField("colourGroups")]
        public List<string> ColourGroups { get; set; }
        [SolrField("ageGroup")]
        public string AgeGroup { get; set; }
        [SolrField("ageGroupIntervals")]
        public IList<string> AgeGroupIntevals { get; set; }
        [SolrField("images")]
        public IList<string> Images { get; set; }
        [SolrField("categories")]
        public IList<string> Categories { get; set; }
        [SolrField("subCategory")]
        public string SubCategory { get; set; }
        [SolrField("price")]
        public int Price { get; set; }
        [SolrField("priceGroup")]
        public string PriceGroup { get; set; }
        [SolrField("stock")]
        public int Stock { get; set; }

        public SolrToy(Toy toy)
        {
            Id = toy.ID;
            ProductId = toy.ProductId;
            Name = toy.Name;
            ShortDescription = toy.ShortDescription;
            LongDescription = toy.LongDescription;
            Categories = toy.SubCategory.Categories.Select(c => c.Name).ToList();
            Price = toy.Price;
            PriceGroup = toy.PriceGroup.Interval;
            Stock = toy.Stock;
            Brand = toy.Brand.Name;
            Colour = toy.Colour;
            ColourGroups = toy.ColourGroups.Select(c => c.Name).ToList();
            AgeGroup = toy.AgeGroup;
            AgeGroupIntevals = toy.AgeGroups.Select(a => a.Interval).ToList();
            Images = toy.Images.Select(i => i.UrlHigh).ToList();
            SubCategory = toy.SubCategory.Name;
        }
    }
}
