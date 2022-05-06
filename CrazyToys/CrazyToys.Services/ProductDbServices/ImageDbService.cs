using CrazyToys.Data.Data;
using CrazyToys.Entities.Entities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Services.ProductDbServices
{
    public class ImageDbService : IEntityCRUD<Image>
    {

        private readonly Context _context;

        public ImageDbService(Context context)
        {
            _context = context;
        }

        public Task<Image> Create(Image t)
        {
            throw new NotImplementedException();
        }

        public Task<Image> CreateOrUpdate(Image t)
        {
            throw new NotImplementedException();
        }

        public Task<List<Image>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<Image>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Image> Update(Image t)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteRange(IList<Image> images)
        {

            _context.Images.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        public Task<Image> Delete(string id)
        {
            throw new NotImplementedException();
        }

     
    }
}
