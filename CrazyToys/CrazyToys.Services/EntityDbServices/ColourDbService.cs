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
    public class ColourDbService : IEntityCRUD<Colour>
    {
        private readonly Context _context;

        public ColourDbService(Context context)
        {
            _context = context;
        }

        public async Task<Colour> Create(Colour colour)
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();

            return colour;
        }

        public Task<List<Colour>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Colour> GetById(string id)
        {
            /*
            if (id != 0)
            {
                var colour = await _context.Colours
                    .FirstOrDefaultAsync(c => c.ID == id);

                return colour;
            }
            */
            return null;

        }

        /*
        public async Task<Colour> Upsert(string name)
        {
            var colourId = _context.Colours.FromSqlRaw("UpsertColour", name);
            return new Colour(colourId, name);
        }
        */

        public Task<Colour> Update(Colour colour)
        {
            throw new NotImplementedException();
        }

        public async Task<Colour> GetByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var colour = await _context.Colours
                    .FirstOrDefaultAsync(c => c.Name == name);

                return colour;
            }
            return null;

        }

        public Task<Colour> CreateOrUpdate(Colour t)
        {
            throw new NotImplementedException();
        }
    }



}
