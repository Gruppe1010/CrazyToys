﻿using CrazyToys.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CrazyToys.Entities.Entities
{
    public class SubCategory
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; }



        public SubCategory(string iD, string name)
        {
            ID = iD;
            Name = name;
            Categories = new List<Category>();
        }

        public SubCategoryDTO convertToDTO()
        {
            return new SubCategoryDTO(ID, Name);
        }

    }
}