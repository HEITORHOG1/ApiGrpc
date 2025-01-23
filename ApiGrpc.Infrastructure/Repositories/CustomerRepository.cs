using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiGrpc.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Customers
                .AnyAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            return await _context.Customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}