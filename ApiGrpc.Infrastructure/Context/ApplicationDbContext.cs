using ApiGrpc.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApiGrpc.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Endereco> Enderecos { get; set; } = null!;
        public DbSet<Estabelecimento> Estabelecimentos { get; set; } = null!;
        public DbSet<HorarioFuncionamento> HorariosFuncionamentos { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}