using System.Collections.Generic;

namespace CrazyToys.Entities.Entities
{
    public class Brand
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public IList<Toy> Toys { get; set; }

        public Brand(string iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public Brand(string iD, string name, string logoUrl) : this(iD, name)
        {
            LogoUrl = logoUrl;
        }
    }
}