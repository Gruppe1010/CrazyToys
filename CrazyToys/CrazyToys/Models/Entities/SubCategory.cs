using CrazyToys.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrazyToys.Models.Entities
{
    public class SubCategory
    {
        [Column("IdSubCategory")]
        public string Id { get; set; } 
        public string Name { get; set; }
        public ICollection<Toy> Toys { get; set; }

        // TODO skal denne have en collection af Category?

        public SubCategoryDTO convertToDTO()
        {
            return new SubCategoryDTO(Id, Name);
        }

    }
}