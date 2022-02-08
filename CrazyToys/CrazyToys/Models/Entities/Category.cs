﻿using CrazyToys.Web.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrazyToys.Web.Models.Entities
{
    public class Category
    {
        [Column("CategoryId")]
        public string ID { get; set; }
        public string Name { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }

        // TODO skal denne have en collection af Category?

    }
}