using Infrastructure.Entities;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Complete();
    }
}
