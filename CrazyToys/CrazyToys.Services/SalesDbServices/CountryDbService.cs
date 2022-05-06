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
    public class CountryDbService : IEntityCRUD<Country>
    {

        private readonly SalesContext _salesContext;

        public CountryDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public Task<Country> Create(Country country)
        {
            throw new NotImplementedException();
        }

        public Task<Country> CreateOrUpdate(Country country)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<Country> countries)
        {
            throw new NotImplementedException();
        }

        public Task<List<Country>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Country>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<Country> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Country> GetByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var country = await _salesContext.Countries
                    .FirstOrDefaultAsync(c => c.Name == name);
                return country;
            }
            return null;
        }

        public Task<Country> Update(Country country)
        {
            throw new NotImplementedException();
        }

        public Task<Country> Delete(string id)
        {
            throw new NotImplementedException();
        }

        
    }
}
