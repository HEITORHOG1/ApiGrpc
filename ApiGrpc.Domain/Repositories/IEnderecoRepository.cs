using ApiGrpc.Domain.Repositories.Base;

namespace ApiGrpc.Domain.Repositories
{
    public interface IEnderecoRepository : IRepository<Endereco>
    {
        Task<IEnumerable<Endereco>> GetByUsuarioAsync(Guid usuarioId);

        Task<IEnumerable<Endereco>> GetEstabelecimentosProximosAsync(double lat, double lng, double raioKm);

        Task<IEnumerable<Endereco>> GetEstabelecimentosAsync();

        Task<bool> ExisteEnderecoUsuarioAsync(Guid usuarioId);
    }
}