using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mispollos.Domain.Contracts.Repositories
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<List<T>> ListAllAsync();

        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> query);

        Task<int> CountByQuery(Expression<Func<T, bool>> query);

        Task<int> Count();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task DisposeAsync();
    }
}