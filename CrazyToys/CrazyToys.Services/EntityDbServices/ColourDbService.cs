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
    public class ColourDbService : IEnitityCRUD<Colour>
    {
        private readonly Context _context;

        public ColourDbService(Context context)
        {
            _context = context;
        }

        public async Task<Colour> Create(Colour colour)
        {
            _context.Colours.Add(colour);
            int noget = await _context.SaveChangesAsync();

            return colour;
        }

        public Task<ICollection<Colour>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Colour> GetById(string id)
        {

            if (!String.IsNullOrWhiteSpace(id))
            {
                var colour = await _context.Colours
                    .FirstOrDefaultAsync(c => c.ID == id);

                return colour;
            }
            return null;
        }

        public Task<Colour> UpdateById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Colour> GetByName(string name)
        {
            if (!String.IsNullOrWhiteSpace(name))
            {
                var colour = await _context.Colours
                    .FirstOrDefaultAsync(c => c.Name == name);

                return colour;
            }
            return null;

        }
    }



}
