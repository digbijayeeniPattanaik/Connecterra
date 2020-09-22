using API.Data;
using Infrastructure.Entities;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FarmContext _farmContext;
        private Hashtable _repositories;

        public UnitOfWork(FarmContext farmContext)
        {
            _farmContext = farmContext;
        }

        public async Task<int> Complete()
        {
            return await _farmContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _farmContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _farmContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
