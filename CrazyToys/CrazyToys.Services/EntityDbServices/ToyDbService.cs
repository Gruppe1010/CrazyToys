using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Entities.SolrModels;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrazyToys.Services.EntityDbServices
{
    public class ToyDbService : IEntityCRUD<Toy>, IToyDbService
    {
        private readonly Context _context;
        private readonly ISearchService<SolrToy> _solrToyService;

        public ToyDbService(Context context, ISearchService<SolrToy> solrToyService)
        {
            _context = context;
            _solrToyService = solrToyService;
        }

        public async Task<Toy> Create(Toy toy)
        {
            _context.Toys.Add(toy);
            await _context.SaveChangesAsync();

            await _solrToyService.CreateOrUpdate(new SolrToy(toy));

            return toy;
        }

        public async Task<Toy> CreateOrUpdate(Toy toy)
        {
            Toy toyFromDb = await GetById(toy.ID);

            if (toyFromDb != null) // TODO lav måske noget her
            {
                toy = await Update(toy);
                _solrToyService.CreateOrUpdate(new SolrToy(toy));
                return toy;
            }
            else
            {
                toy = await Create(toy);
                _solrToyService.CreateOrUpdate(new SolrToy(toy));
                return toy;
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
                .Include(t => t.Images) 
                .Include(t => t.Brand)
                .Include(t => t.SubCategory)
                .FirstOrDefaultAsync(t => t.ID.Equals(id));


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
            var query = from colour in _context.Colours
                        where colour.Toys.Any(t => t.ID.Equals(toyId))
                        select colour;

            var colours = await query.ToListAsync();

            return colours;
        }

        public async Task<List<AgeGroup>> GetAgeGroups(string toyId)
        {
            var query = from ageGroup in _context.AgeGroups
                        where ageGroup.Toys.Any(t => t.ID.Equals(toyId))
                        select ageGroup;

            var ageGroups = await query.ToListAsync();

            return ageGroups;
        }

        public async Task<Toy> Update(Toy toy)
        {
            _context.Update(toy);
            await _context.SaveChangesAsync();
            await _solrToyService.CreateOrUpdate(new SolrToy(toy));

            return toy;
        }

        public async Task<Toy> GetByProductIdAndBrandId(string productId, string brandId)
        {
            if (productId == null)
            {
                return null;
            }

            var toy = await _context.Toys
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.ProductId.Equals(productId) && t.Brand.ID.Equals(brandId));

            return toy;
        }

        public async Task<List<Toy>> GetAllWithRelations()
        {
            var toys = await _context.Toys
                .Include(t => t.Images)
                .Include(t => t.SubCategory)
                .Where(t => t.SimpleToy.OnMarket.Equals("1") && t.Stock != 0)
                .ToListAsync();

            foreach (var toy in toys)
            {
                toy.SubCategory.Categories = await _context.Categories
                    .Include(s => s.SubCategories)
                    .Where(s => s.SubCategories.Contains(toy.SubCategory))
                    .ToListAsync();
                toy.SubCategory.Categories.ToList().ForEach(c => c.SubCategories = null);
            }
            return toys;
        }

        public Task DeleteRange(IList<Toy> tList)
        {
            throw new NotImplementedException();
        }
    }
}
