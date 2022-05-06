using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class PriceGroupDbService : IEntityCRUD<PriceGroup>
    {
        private readonly Context _context;

        public PriceGroupDbService(Context context)
        {
            _context = context;
        }

        public Task<PriceGroup> Create(PriceGroup t)
        {
            throw new NotImplementedException();
        }

        public Task<PriceGroup> CreateOrUpdate(PriceGroup t)
        {
            throw new NotImplementedException();
        }

        public Task<PriceGroup> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<PriceGroup> tList)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PriceGroup>> GetAll()
        {
            return await _context.PriceGroups.ToListAsync();
        }

        public Task<List<PriceGroup>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<PriceGroup> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PriceGroup> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<PriceGroup> Update(PriceGroup t)
        {
            throw new NotImplementedException();
        }

    }
}
