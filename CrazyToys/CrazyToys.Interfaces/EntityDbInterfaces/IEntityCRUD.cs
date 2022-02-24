﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyToys.Interfaces.EntityDbInterfaces
{
    // I hver service som implementerer dette interface, sender den den type Enititet tilbage i stedet for Object
    public interface IEntityCRUD<T>
    {
        Task<List<T>> GetAll();

        Task<List<T>> GetAllWithRelations();

        Task<T> GetById(string id);

        Task<T> GetByName(string name);

        Task<T> Update(T t);

        Task<T> Create(T t);

        Task<T> CreateOrUpdate(T t);

        Task DeleteRange(IList<T> tList);

        //Task<Object> Delete(string id);






    }
}
