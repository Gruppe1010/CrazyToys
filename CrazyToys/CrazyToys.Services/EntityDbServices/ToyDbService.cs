using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.EntityDbServices
{
    public class ToyDbService : IEntityCRUD<Toy>
    {
        private readonly Context _context;

        public ToyDbService(Context context)
        {
            _context = context;
        }

        public async Task<Toy> Create(Toy toy)
        {
            _context.Toys.Add(toy);
            await _context.SaveChangesAsync();

            return toy;
        }

        public Task<List<Toy>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Toy> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Toy> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Toy> UpdateById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
