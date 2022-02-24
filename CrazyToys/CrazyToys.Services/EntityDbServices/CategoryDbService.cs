using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.EntityDbServices
{
    public class CategoryDbService : IEntityCRUD<Category>
    {

        private readonly Context _context;

        public CategoryDbService(Context context)
        {
            _context = context;
        }

        public Task<Category> Create(Category t)
        {
            throw new NotImplementedException();
        }

        public Task<Category> CreateOrUpdate(Category t)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<Category> tList)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Category>> GetAllWithRelations()
        {
            List<Category> categories = await _context.Categories.Include(c => c.SubCategories).ToListAsync();
            // vi sletter Categories-listen på alle subcats for ikke at ende i uendelighedsloop
            foreach (Category cat in categories)
            {
                foreach (SubCategory subCat in cat.SubCategories)
                {
                    subCat.Categories = null;
                }
            }
            return categories;
        }

        public Task<Category> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Category> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Category> Update(Category t)
        {
            throw new NotImplementedException();
        }
    }
}
