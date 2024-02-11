using FeedbackAnalysis.Domain.Abstractions.Repository.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FeedbackAnalysis.Persistance.Repositories
{
    public class BaseRepository<T> : IRepository<T>
        where T : class
    {
        protected readonly FeedbackDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(FeedbackDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();

        public ValueTask<EntityEntry<T>> AddAsync(T entity) => _dbSet.AddAsync(entity);

        public void Update(T entity) => _dbSet.Update(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);

        public Task AddRangeAsync(IEnumerable<T> entities) => _dbSet.AddRangeAsync(entities);

        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool track = false)
            => GetQuery(track, predicate, include);

        public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool track = true)
        {
            var query = GetQuery(track, predicate, include);
            return await query.FirstOrDefaultAsync();
        }

        private IQueryable<T> GetQuery(bool track, Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Expression<Func<T, T>> selector = null)
        {
            IQueryable<T> query = _dbSet;
            if (!track)
            {
                query = query.AsNoTracking();
            }
            if (include != null)
            {
                query = include(query);
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (selector != null)
            {
                query = query.Select(selector);
            }
            return query;
        }
    }
}
