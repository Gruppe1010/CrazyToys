using CrazyToys.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrazyToys.Entities.DTOs
{
    public class ToyDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Brand Brand { get; set; }
        public string AgeGroup { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public Colour Colour { get; set; }
        public ICollection<AgeGroupDTO> AgeGroupDTOs { get; set; }
        public ICollection<SubCategoryDTO> SubCategoryDTOs { get; set; }
        public int Price { get; set; }
        public int Stock { get; set; }
        public ICollection<Image> Images { get; set; }

        public ToyDTO(
            string id, string name, Brand brand, string ageGroup, string shortDescription,
            string longDescription,
            /*
            ICollection<Colour> colours,
            ICollection<AgeGroupDTO> ageGroupDTOs, 
            ICollection<SubCategoryDTO> subCategoryDTOs, */ // TODO opret convertere og find ud af hvordan man siger map
            int price, int stock, ICollection<Image> images)
        {
            Id = id;
            Name = name;
            Brand = brand;
            AgeGroup = ageGroup;
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            /*
            Colours = colours;
            AgeGroupDTOs = ageGroupDTOs;
            SubCategoryDTOs = subCategoryDTOs;*/
            Price = price;
            Stock = stock;
            Images = images;
        }
    }





}