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
    public class CityDbService
    {
        private readonly SalesContext _salesContext;

        public CityDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public async Task<City> GetByPostalCode(string postalCode)
        {
            if (!String.IsNullOrWhiteSpace(postalCode))
            {
                var city = await _salesContext.Cities
                    .FirstOrDefaultAsync(o => o.PostalCode.Equals(postalCode));
                return city;
            }
            return null;
        }
    }
}
