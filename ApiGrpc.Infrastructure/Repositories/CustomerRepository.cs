using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(Guid id)
            => await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Customer> GetByEmailAsync(string email)
            => await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

        public async Task<IEnumerable<Customer>> GetAllAsync()
            => await _context.Customers.ToListAsync();

        public async Task<Customer> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
            => await _context.Customers.AnyAsync(c => c.Email == email);
    }
}