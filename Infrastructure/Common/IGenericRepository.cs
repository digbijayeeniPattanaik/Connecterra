using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetListAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
