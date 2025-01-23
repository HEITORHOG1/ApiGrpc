using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Infrastructure.Repositories
{
    public class EnderecoRepository : BaseRepository<Endereco>, IEnderecoRepository
    {
        private readonly ApplicationDbContext _context;

        public EnderecoRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Endereco>> GetByUsuarioAsync(Guid usuarioId)
        {
            return await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Endereco>> GetEstabelecimentosProximosAsync(double lat, double lng, double raioKm)
        {
            return await _context.Enderecos
                .Where(e => e.IsEstabelecimento && e.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Endereco>> GetEstabelecimentosAsync()
        {
            return await _context.Enderecos
                .Where(e => e.IsEstabelecimento && e.Status)
                .ToListAsync();
        }

        public async Task<bool> ExisteEnderecoUsuarioAsync(Guid usuarioId)
        {
            return await _context.Enderecos
                .AnyAsync(e => e.UsuarioId == usuarioId);
        }
    }
}