using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.OrderEntities
{
    public class City
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ID { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }

        public City(string iD, string cityName, string postalCode)
        {
            ID = iD;
            CityName = cityName;
            PostalCode = postalCode;
        }
    }
}
