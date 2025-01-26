using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Infrastructure.Repositories
{
    public class EstabelecimentoRepository : BaseRepository<Estabelecimento>, IEstabelecimentoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstabelecimentoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Estabelecimento>> GetAll()
        {
            return await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .Include(e => e.HorarioFuncionamento)
                .Include(e => e.Categoria)
                .ToListAsync();
        }
        public async Task<Estabelecimento?> GetById(Guid id)
        {
            return await _context.Estabelecimentos
                .Include(e => e.Endereco)
                .Include(e => e.HorarioFuncionamento)
                .Include(e => e.Categoria)
                .FirstOrDefaultAsync(e => e.Id == id);
        }
        public async Task<IEnumerable<Estabelecimento>> GetEstabelecimentosByUsuarioAsync(Guid usuarioId)
        {
            return await _context.Estabelecimentos
                .Where(e => e.UsuarioId == usuarioId)
                .Include(e => e.Endereco) // Adicionado
                .Include(e => e.Categoria) // Adicionado
                .Include(e => e.HorarioFuncionamento) // Adicionado
                .ToListAsync();
        }

        public async Task<IEnumerable<Estabelecimento>> GetEstabelecimentosByCategoriaAsync(Guid categoriaId)
        {
            return await _context.Estabelecimentos
                .Include(e => e.Categoria)
                .Include(e => e.Endereco)
                .Where(e => e.CategoriaId == categoriaId)
                .ToListAsync();
        }

        public async Task<bool> CNPJExisteAsync(string cnpj)
        {
            return await _context.Estabelecimentos
                .AnyAsync(e => e.CNPJ == cnpj);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _context.Estabelecimentos
                .AnyAsync(e => e.Email == email);
        }
    }
}