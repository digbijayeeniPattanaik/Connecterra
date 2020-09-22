using API.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly FarmContext _farmContext;

        public GenericRepository(FarmContext farmContext)
        {
            _farmContext = farmContext;
        }
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _farmContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetListAllAsync()
        {
            return await _farmContext.Set<T>().ToListAsync(); 
        }

        public void Update(T entity)
        {
            _farmContext.Set<T>().Attach(entity);
            _farmContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
