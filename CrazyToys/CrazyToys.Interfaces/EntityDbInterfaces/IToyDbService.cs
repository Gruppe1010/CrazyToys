﻿using CrazyToys.Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    public interface IToyDbService
    {
        Task<Toy> GetByProductIdAndBrandId(string productId, string brandId);

        Task<List<ColourGroup>> GetColours(string toyId);

        Task<List<AgeGroup>> GetAgeGroups(string toyId);

        Task<Toy> GetByProductId(string productId);

        Task<Toy> CreateOrUpdate(Toy toy);

        Task<Toy> Create(Toy toy);

        Task<List<Toy>> GetAll();

        Task<Toy> GetById(string id);

        Task<Toy> Update(Toy toy);

        Task<List<Toy>> GetAllWithRelations();

        Task<List<Toy>> GetToysToUpdateWithRelations(string dateString);
    }
}
