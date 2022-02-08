
using CrazyToys.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrazyToys.Web.Models.Entities
{
    public class SubCategory
    {
        [Column("SubCategoryId")]
        public string ID { get; set; } 
        public string Name { get; set; }
        public ICollection<Toy> Toys { get; set; }
        public ICollection<Category> Categories { get; set; }


        // TODO skal denne have en collection af Category?

        public SubCategoryDTO convertToDTO()
        {
            return new SubCategoryDTO(ID, Name);
        }

    }
}