using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class SubCategoryDbService : ISubCategoryDbService
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
    }
}
