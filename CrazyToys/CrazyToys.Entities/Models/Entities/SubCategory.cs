using CrazyToys.Entities.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrazyToys.Entities.Models.Entities
{
    public class SubCategory
    {
        [Column("SubCategoryId")]
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }

        public SubCategory(string iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public SubCategoryDTO convertToDTO()
        {
            return new SubCategoryDTO(ID, Name);
        }

    }
}