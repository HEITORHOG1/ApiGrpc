using System.Linq.Expressions;

namespace ApiGrpc.Domain.Repositories.Base
{
    public interface IRepository<T> where T : class
    {
        // Operações Básicas
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Operações de Escrita
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        Task UpdateAsync(T entity); // Alterado para Task (assíncrono)

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
    }
}