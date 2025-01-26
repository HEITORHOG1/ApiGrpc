using ApiGrpc.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ApiGrpc.Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
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

            // Estabelecimento 1:1 Endereco
            modelBuilder.Entity<Estabelecimento>()
                .HasOne(e => e.Endereco)
                .WithOne(e => e.Estabelecimento)
                .HasForeignKey<Endereco>(e => e.EstabelecimentoId);

            // Usuario 1:N Endereco
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Enderecos)
                .WithOne(e => e.Usuario)
                .HasForeignKey(e => e.UsuarioId);

            // Estabelecimento 1:N HorarioFuncionamento
            modelBuilder.Entity<Estabelecimento>()
                .HasMany(e => e.HorariosFuncionamento)
                .WithOne(h => h.Estabelecimento)
                .HasForeignKey(h => h.EstabelecimentoId);

            // Categoria 1:N Estabelecimento
            modelBuilder.Entity<Categoria>()
                .HasMany<Estabelecimento>()
                .WithOne(e => e.Categoria)
                .HasForeignKey(e => e.CategoriaId);

            modelBuilder.Entity<Endereco>()
               .HasOne(e => e.Usuario)
               .WithMany(u => u.Enderecos)
               .HasForeignKey(e => e.UsuarioId)
               .IsRequired(false);

            modelBuilder.Entity<Endereco>()
                .HasOne(e => e.Estabelecimento)
                .WithOne(e => e.Endereco)
                .HasForeignKey<Endereco>(e => e.EstabelecimentoId)
                .IsRequired(false);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}