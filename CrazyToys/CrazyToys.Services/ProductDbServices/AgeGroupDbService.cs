using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class AgeGroupDbService : IEntityCRUD<AgeGroup>
    {

        private readonly Context _context;

        public AgeGroupDbService(Context context)
        {
            _context = context;
        }

        public Task<AgeGroup> Create(AgeGroup t)
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroup> CreateOrUpdate(AgeGroup t)
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroup> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<AgeGroup> tList)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AgeGroup>> GetAll()
        {
            return await _context.AgeGroups.ToListAsync();
        }

        public Task<List<AgeGroup>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroup> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroup> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<AgeGroup> Update(AgeGroup t)
        {
            throw new NotImplementedException();
        }
    }
}
