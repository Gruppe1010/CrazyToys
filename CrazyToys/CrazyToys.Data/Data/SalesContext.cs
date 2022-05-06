using CrazyToys.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore;


namespace CrazyToys.Data.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }

        public SalesContext(DbContextOptions<SalesContext> options) : base(options)
        {
        }

    }
}

