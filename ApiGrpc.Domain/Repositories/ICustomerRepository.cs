using ApiGrpc.Domain.Entities;

namespace ApiGrpc.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(Guid id);

        Task<Customer> GetByEmailAsync(string email);

        Task<IEnumerable<Customer>> GetAllAsync();

        Task<Customer> AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);

        Task DeleteAsync(Customer customer);

        Task<bool> EmailExistsAsync(string email);
    }
}