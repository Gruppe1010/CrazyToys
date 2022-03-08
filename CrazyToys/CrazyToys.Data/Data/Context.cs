using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyToys.Entities.Entities;
using Microsoft.EntityFrameworkCore;


namespace CrazyToys.Data.Data
{
    public class Context : DbContext
    {
        public DbSet<ColourGroup> Colours { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<PriceGroup> PriceGroups { get; set; }
        public DbSet<ColourGroup> ColourGroups { get; set; }

        public DbSet<Toy> Toys { get; set; }
        public DbSet<SimpleToy> SimpleToys { get; set; }
        public DbSet<Error> Errors { get; set; }


        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

    }
}

