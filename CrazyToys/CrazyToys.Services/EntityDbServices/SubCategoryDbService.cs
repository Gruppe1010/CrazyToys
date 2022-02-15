using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.EntityDbServices
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

        public Task<List<SubCategory>> GetAll()
        {
            throw new NotImplementedException();
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
