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

        public async Task<Toy> CreateOrUpdate(Toy toy)
        {
            Toy toyFromDb = await GetById(toy.ID);

            if(toyFromDb != null)
            {
                return await Update(toy);
            }
            else
            {
                return await Create(toy);
            }

        }

        public Task<List<Toy>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Toy> GetById(string id)
        {
            if (id == null)
            {
                return null;
            }

            var toy = await _context.Toys
                .FirstOrDefaultAsync(b => b.ID.Equals(id));


            //var images = await _context.Images.Where(x => x.ToyID.Equals(id)).ToListAsync();
            //var colours = await _context.ColourToy.Where(x => x.ColourToy == id).ToListAsync();
            //var ageGroups = await _context.AgeGroups.Where(x => x.ID == id).ToListAsync();

            //_context.ChangeTracker.Clear(); // TODO er vi sikre på at vi skal have denne?

            return toy;
        }

        public Task<Toy> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Toy> Update(Toy toy)
        {
            try { 
                _context.Update(toy);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToyExists(toy.ID))
                {
                    // noget
                }
               
            }
            return toy;
        }

        private bool ToyExists(string id)
        {
            return _context.Toys.Any(e => e.ID.Equals(id));
        }
    }
}
