using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FeedbackAnalysis.Domain.Abstractions.Repository.Base
{
    public interface IRepository<T>
        where T : class
    {
        Task SaveChangesAsync();

        ValueTask<EntityEntry<T>> AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        void RemoveRange(IEnumerable<T> entities);

        IQueryable<T> GetAll(
           Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           bool track = false);

        Task<T> GetFirstOrDefaultAsync(
           Expression<Func<T, bool>> predicate = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
           bool track = true);
    }
}
