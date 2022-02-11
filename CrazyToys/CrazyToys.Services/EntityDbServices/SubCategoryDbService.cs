﻿using CrazyToys.Data.Data;
using CrazyToys.Entities.Models.Entities;
using CrazyToys.Interfaces;
using CrazyToys.Interfaces.EntityDbInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Services
{
    public class SubCategoryDbService : IEnitityCRUD<SubCategory>
    {
        private readonly Context _context;

        public SubCategoryDbService(Context context)
        {
            _context = context;
        }

        public async Task<SubCategory> Create(SubCategory subCategory)
        {

            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();

            return subCategory;
        }

        public Task<ICollection<SubCategory>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<SubCategory> GetById(string id)
        {
            if (id == null)
            {
                return null;
            }

            var subCategory = await _context.SubCategories
                .FirstOrDefaultAsync(b => b.ID == id);
            return subCategory;
        }

        public Task<SubCategory> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<SubCategory> UpdateById(string id)
        {
            throw new NotImplementedException();
        }
    }



}