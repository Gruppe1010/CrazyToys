using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrazyToys.Entities.DTOs

{
    public class SubCategoryDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }


        public SubCategoryDTO(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}