using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories.Base;

namespace ApiGrpc.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<IEnumerable<Customer>> GetAllAsync(int pageNumber = 1, int pageSize = 10);

        Task<Customer?> GetByEmailAsync(string email);

        Task<bool> EmailExistsAsync(string email);
    }
}