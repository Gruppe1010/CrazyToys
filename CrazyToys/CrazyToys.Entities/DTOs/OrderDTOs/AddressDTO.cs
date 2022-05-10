using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Entities.DTOs.OrderDTOs
{
    public class AddressDTO
    {
        public string StreetAddress { get; set; }
        public string City { get; set; } // "2300, København"
        public string Country { get; set; }

        public AddressDTO(string streetAddress, string city, string country)
        {
            StreetAddress = streetAddress;
            City = city;
            Country = country;
        }
    }
}
