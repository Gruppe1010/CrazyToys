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
    public class BrandDbService : IEntityCRUD<Brand>
    {
        private readonly Context _context;

        public BrandDbService(Context context)
        {
            _context = context;
        }

        public Task<Brand> Create(Brand brand)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> CreateOrUpdate(Brand t)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Brand>> GetAll()
        {
            return await _context.Brands.ToListAsync();

        }

        public async Task<Brand> GetById(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                var brand = await _context.Brands
                    .FirstOrDefaultAsync(b => b.ID == id);
                return brand;
            }
            return null;
        }

        public Task<Brand> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Brand> Update(Brand t)
        {
            throw new NotImplementedException();
        }
    }



}
