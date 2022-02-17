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
    public class ToyDbService : IEntityCRUD<Toy>, IToyDbService
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

            if (toyFromDb != null)
            {
                return await Update(toy);
            }
            else
            {
                return await Create(toy);
            }

        }

        public async Task<List<Toy>> GetAll()
        {
            return await _context.Toys.ToListAsync();
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

        public async Task<List<Colour>> GetColours(string toyId)
        {
            //var cat_id = 1; // Change this variable for your real cat_id

            var query = from colour in _context.Colours
                        where colour.Toys.Any(t => t.ID.Equals(toyId))
                        select colour;

            var colours = await query.ToListAsync();

            return colours;


        }

        public bool HasColour(string toyId, int colourId)
        {
            //var colours = await _context.Toys.Where(toy => toy.ID == toyId && ).ToListAsync();
            /*

            var noget = _context.Toys
            .Join(
                _context.Colours,
                toy => toy.ID,
                colour => colour.ID.CustomerId,
                (customer, invoice) => new
                {
                    InvoiceID = invoice.Id,
                    CustomerName = customer.FirstName + "" + customer.LastName,
                    InvoiceDate = invoice.Date
                }
            ).ToListAsync();
            */


            var nogetAndet = _context.Toys
            .Where(toy => toy.ID.Equals(toyId))                      // only if you don't want all Users
            .Select(toy => new
            {    // Select only the properties you plan to use:
                Colours = toy.Colours
                     .Where(colour => colour.ID == colourId)         // only if you don't want all profiles
                     .Select(colour => new
                     {
                         ID = colour.ID
                     })
                     .ToList(),
            });



            foreach (var item in nogetAndet)
            {
            }


            var hej = 1;
            return false;
        }

        public async Task<Toy> Update(Toy toy)
        {
            _context.Update(toy);
            await _context.SaveChangesAsync();

            return toy;
        }

    }
}
