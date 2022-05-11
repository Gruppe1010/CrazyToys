using CrazyToys.Data.Data;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.SalesDbServices
{
    public class AddressDbService
    {

        private readonly SalesContext _salesContext;

        public AddressDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }


        public async Task<Address> GetByCityAndStreetAddress(City city, string streetAddress)
        {
            if (city != null && !String.IsNullOrWhiteSpace(streetAddress))
            {
                var address = await _salesContext.Addresses
                    .FirstOrDefaultAsync(o => o.City.Equals(city) && o.StreetAddress.Replace(" ", "").Equals(streetAddress.Replace(" ", "")));
                return address;
            }
            return null;
        }

     
    }
}
