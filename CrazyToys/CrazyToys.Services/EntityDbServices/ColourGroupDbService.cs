using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.EntityDbServices
{
    public class ColourGroupDbService : IEntityCRUD<ColourGroup>
    {
        private readonly Context _context;

        public ColourGroupDbService(Context context)
        {
            _context = context;
        }

        public async Task<ColourGroup> Create(ColourGroup colour)
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();

            return colour;
        }

        public async Task<List<ColourGroup>> GetAll()
        {
            return await _context.ColourGroups.ToListAsync();
        }

        public Task<ColourGroup> GetById(string id)
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

        public Task<ColourGroup> Update(ColourGroup colour)
        {
            throw new NotImplementedException();
        }

        public async Task<ColourGroup> GetByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var colour = await _context.Colours
                    .FirstOrDefaultAsync(c => c.Name == name);

                return colour;
            }
            return null;

        }

        public Task<ColourGroup> CreateOrUpdate(ColourGroup t)
        {
            throw new NotImplementedException();
        }

        public Task<List<ColourGroup>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<ColourGroup> tList)
        {
            throw new NotImplementedException();
        }
    }



}
