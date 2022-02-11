using CrazyToys.Entities.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CrazyToys.Data.Data
{
    public class DbInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();

            if (!context.Brands.Any())
            {
                var brands = new Brand[]
                {
                    new Brand("15111", "Barbie", "https://images.icecat.biz/img/brand/thumb/15111_e16ba4de456d43bd993b5c39607aa845.jpg"),
                    new Brand("5669", "Hasbro", "https://images.icecat.biz/img/brand/thumb/5669_5c735b62136a4d32b553ed74c66cdb15.jpg"),
                    new Brand("27814", "Bambolino", "https://images.icecat.biz/img/brand/thumb/27814_29d6d8a1fda04558a4cae1a0b9a7d175.jpg"),
                    new Brand("23505", "Clown", "https://images.icecat.biz/img/brand/thumb/23505_3387646289f8463ebca32c8b5fded65b.jpg"),
                    new Brand("32046", "Fuzzikins Fuzzi", "https://images.icecat.biz/img/brand/thumb/32046_d6cefd82d5714ed08af17b4eea0f3237.jpg"),
                    new Brand("7375", "IMC Toys", "https://images.icecat.biz/img/brand/thumb/7375_62ea0b83ce6e454daf6b5a5910c53d87.jpg"),
                    new Brand("24094", "LumoStars", "https://images.icecat.biz/img/brand/thumb/24094_277b2a8a5f75443ababf44ae56acf1cd.jpg"),
                    new Brand("36068", "My Little Pony", "https://images.icecat.biz/img/brand/thumb/36068_33f45a22fbb042ff979cc93c18f17c00.jpg"),
                    new Brand("16933", "Play-Doh", "https://images.icecat.biz/img/brand/thumb/16933_fd0ecf980706469dbc94333be9e1e435.jpg"),
                    new Brand("15136", "Polly Pocket", "https://images.icecat.biz/img/brand/thumb/15136_55e19fa2894d4273810836c223673f4c.jpg"),
                    new Brand("16046", "SESCreative", "https://images.icecat.biz/img/brand/thumb/16046_dd661f6b89de45819bb760a18e9c81ef.jpg"),
                    new Brand("23442", "RuboToys", "https://images.icecat.biz/img/brand/thumb/23442_16e7f8ad9bde491c8619a1d68d09c25a.jpg"),
                    new Brand("3480", "Jumbo", "https://images.icecat.biz/img/brand/thumb/3480_998702f9778f4b2cad3b2b69d98ee481.jpg")
                };

                foreach (Brand brand in brands)
                {
                    context.Brands.Add(brand);
                }
            }
 

            if (!context.AgeGroups.Any())
            {
                var ageGroups = new AgeGroup[]
                {
                    new AgeGroup("0-11 måneder"),
                    new AgeGroup("1-2 år"),
                    new AgeGroup("3-4 år"),
                    new AgeGroup("5-6 år"),
                    new AgeGroup("7-8 år"),
                    new AgeGroup("9+")

                };
                foreach (AgeGroup ageGroup in ageGroups)
                {
                    context.AgeGroups.Add(ageGroup);
                }
            }

            if(!context.Categories.Any())
            {
                var categories = new Category[]
                { 
                    new Category("Dukker"),
                    new Category("Figurer"),
                    new Category("Bamser"),
                    new Category("Kreativ"),
                    new Category("Spil"),
                    new Category("Assorteret")

                };
                foreach (Category category in categories)
                {
                    context.Categories.Add(category);
                }
            }

            context.SaveChanges();
        }
    }
}
