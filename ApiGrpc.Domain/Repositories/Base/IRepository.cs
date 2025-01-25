using System.Linq.Expressions;

namespace ApiGrpc.Domain.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity);

        Task Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}