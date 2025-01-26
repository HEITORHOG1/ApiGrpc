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
                .Where(e => e.UsuarioId == usuarioId && e.Status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Endereco>> GetEstabelecimentosProximosAsync(double lat, double lng, double raioKm)
        {
            const double EARTH_RADIUS = 6371; // km

            return await _context.Enderecos
                .Where(e => e.IsEstabelecimento && e.Status)
                .AsNoTracking()
                .ToListAsync()
                .ContinueWith(t => t.Result.Where(e =>
                {
                    var dLat = ToRad(e.Latitude - lat);
                    var dLon = ToRad(e.Longitude - lng);
                    var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                            Math.Cos(ToRad(lat)) * Math.Cos(ToRad(e.Latitude)) *
                            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
                    var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                    var distance = EARTH_RADIUS * c;
                    return distance <= raioKm;
                }));
        }

        private static double ToRad(double degrees) => degrees * Math.PI / 180;

        public async Task<IEnumerable<Endereco>> GetEstabelecimentosAsync()
        {
            return await _context.Enderecos
                .Where(e => e.IsEstabelecimento && e.Status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExisteEnderecoUsuarioAsync(Guid usuarioId)
        {
            return await _context.Enderecos
                .AnyAsync(e => e.UsuarioId == usuarioId && e.Status);
        }

        public async Task<IEnumerable<Endereco>> GetByEstabelecimentoAsync(Guid estabelecimentoId)
        {
            return await _context.Enderecos
                .Where(e => e.EstabelecimentoId == estabelecimentoId && e.Status)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExisteEnderecoEstabelecimentoAsync(Guid estabelecimentoId)
        {
            return await _context.Enderecos
                .AnyAsync(e => e.EstabelecimentoId == estabelecimentoId && e.Status);
        }
    }
}