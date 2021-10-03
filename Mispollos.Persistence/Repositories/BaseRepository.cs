using Microsoft.EntityFrameworkCore;
using Mispollos.Domain.Contracts.Repositories;
using Mispollos.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mispollos.Domain.Repositories
{
    public class BaseRepository<T> : IAsyncRepository<T> where T : class
    {
        protected readonly MisPollosContext _dbContext;

        public BaseRepository(MisPollosContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async virtual Task<List<T>> GetPagedReponseAsync(int page, int size)
        {
            return await _dbContext.Set<T>().Skip((page - 1) * size).Take(size).AsNoTracking().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            T entity = await _dbContext.Set<T>().FindAsync(id);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCompositeAsync(Guid id, Guid secondKey)
        {
            T entity = await _dbContext.Set<T>().FindAsync(id, secondKey);
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _dbContext.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public IQueryable<T> Query()
        {
            return _dbContext.Set<T>().AsQueryable();
        }

        public async Task<int> CountByQuery(Expression<Func<T, bool>> query)
        {
            return await _dbContext.Set<T>().CountAsync(query);
        }

        public async Task<int> Count()
        {
            return await _dbContext.Set<T>().CountAsync();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> query)
        {
            return _dbContext.Set<T>().Where(query).AsQueryable();
        }
    }
}