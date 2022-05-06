using CrazyToys.Data.Data;
using CrazyToys.Entities.OrderEntities;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services.SalesDbServices
{
    public class StatusTypeDbService : IEntityCRUD<StatusType>
    {

        private readonly SalesContext _salesContext;

        public StatusTypeDbService(SalesContext salesContext)
        {
            _salesContext = salesContext;
        }

        public Task<StatusType> Create(StatusType statusType)
        {
            throw new NotImplementedException();
        }

        public Task<StatusType> CreateOrUpdate(StatusType statusType)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IList<StatusType> statusType)
        {
            throw new NotImplementedException();
        }

        public async Task<List<StatusType>> GetAll()
        {
            return await _salesContext.StatusTypes.ToListAsync();
        }

        public Task<List<StatusType>> GetAllWithRelations()
        {
            throw new NotImplementedException();
        }

        public Task<StatusType> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<StatusType> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<StatusType> Update(StatusType statusType)
        {
            throw new NotImplementedException();
        }

        public Task<StatusType> Delete(string id)
        {
            throw new NotImplementedException();
        }
    }
}
