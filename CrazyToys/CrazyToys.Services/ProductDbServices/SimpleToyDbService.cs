using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class SimpleToyDbService : IEntityCRUD<SimpleToy>, ISimpleToyDbService
    {
        private readonly Context _context;

        public SimpleToyDbService(Context context)
        {
            _context = context;
        }

        public async Task<SimpleToy> Create(SimpleToy simpleToy)
        {
            _context.SimpleToys.Add(simpleToy);
            await _context.SaveChangesAsync();

            return simpleToy;
        }

        public Task<SimpleToy> CreateOrUpdate(SimpleToy t)
        {
            throw new NotImplementedException();
        }

        public Task<List<SimpleToy>> GetAll()
        {
            throw new NotImplementedException();
        }

        public HashSet<SimpleToy> GetAllAsHashSet()
        {
            return _context.SimpleToys.ToHashSet();
        }

        public HashSet<SimpleToy> GetAllByDate(string dateString)
        {
            if (!string.IsNullOrWhiteSpace(dateString))
            {
                return _context.SimpleToys.Where(s => s.DateString == dateString).ToHashSet();
            }
            return null;
        }

        public Task<List<SimpleToy>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<SimpleToy> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SimpleToy> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<SimpleToy> Update(SimpleToy simpleToy)
        {
            _context.Update(simpleToy);
            await _context.SaveChangesAsync();
            return simpleToy;
        }

        public async Task<SimpleToy> GetByProductIcecatId(string icecatId)
        {
            return await _context.SimpleToys
                    .FirstOrDefaultAsync(s => s.IcecatId.Equals(icecatId));
        }

        public Task DeleteRange(IList<SimpleToy> tList)
        {
            throw new NotImplementedException();
        }

        public Task<SimpleToy> Delete(string id)
        {
            throw new NotImplementedException();
        }

       
    }
}
