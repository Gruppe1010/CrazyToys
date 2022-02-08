using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyToys.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace CrazyToys.Web.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Colour> Colours { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AgeGroup> AgeGroups { get; set; }
        public DbSet<Toy> Toys { get; set; }


       

    }
}

