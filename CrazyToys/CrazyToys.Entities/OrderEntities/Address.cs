using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class Address
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public City City { get; set; }
        public string Street { get; set; }
        public Country Country { get; set; }

        public Address()
        {
        }

        public Address(City city, string street, Country country)
        {
            City = city;
            Street = street;
            Country = country;
        }
    }
}
