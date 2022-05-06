using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class SubCategoryDbService : IEntityCRUD<SubCategory>
    {
        private readonly Context _context;

        public SubCategoryDbService(Context context)
        {
            _context = context;
        }

        public async Task<SubCategory> Create(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();

            return subCategory;
        }

        public Task<SubCategory> CreateOrUpdate(SubCategory t)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<SubCategory> tList)
        {
            throw new NotImplementedException();
        }

        public Task<List<SubCategory>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubCategory>> GetAllWithRelations()
        {
            return await _context.SubCategories
                .Include(s => s.Categories)
                .ToListAsync();
        }

        public async Task<SubCategory> GetById(string id)
        {
            if (id == null)
            {
                return null;
            }

            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(b => b.ID == id);
            return subCategory;
        }

        public Task<SubCategory> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory> Update(SubCategory t)
        {
            throw new NotImplementedException();
        }
    }
}
