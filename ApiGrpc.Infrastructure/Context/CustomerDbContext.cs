using ApiGrpc.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Infrastructure.Context
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}