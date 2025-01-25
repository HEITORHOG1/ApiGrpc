using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Infrastructure.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteCategoriaPorNomeAsync(string nome)
        {
            return await _context.Categorias
                .AnyAsync(c => c.Nome.ToLower() == nome.ToLower());
        }

        public async Task<IEnumerable<Categoria>> GetByEstabelecimentoAsync()
        {
            return await _context.Categorias
                .ToListAsync();
        }
    }
}