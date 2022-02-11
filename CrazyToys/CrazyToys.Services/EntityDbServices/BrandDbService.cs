using CrazyToys.Data.Data;
using CrazyToys.Entities.Models.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class BrandDbService : IEnitityCRUD<Brand>
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

        public Task<ICollection<Brand>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Brand> GetById(string id)
        {
            if (!String.IsNullOrWhiteSpace(id))
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

        public Task<Brand> UpdateById(string id)
        {
            throw new NotImplementedException();
        }
    }



}
