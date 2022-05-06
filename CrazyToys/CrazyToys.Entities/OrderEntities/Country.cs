using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public  class Country
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }

        public Country()
        {
        }

        public Country(string name, string countryCode)
        {
            Name = name;
            CountryCode = countryCode;
        }
    }
}
