using Infrastructure.Data.Specifications;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetListAllAsync();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        List<T> QueryFromSqlRawReturnList(string sqlQuery, SqlParameter[] sqlParameters);
        T QueryFromSqlRaw(string sqlQuery, SqlParameter[] sqlParameters);
    }
}
