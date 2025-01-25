using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories.Base;

namespace ApiGrpc.Domain.Repositories
{
    public interface IEstabelecimentoRepository : IRepository<Estabelecimento>
    {
        Task<IEnumerable<Estabelecimento>> GetEstabelecimentosByUsuarioAsync(Guid usuarioId);

        Task<IEnumerable<Estabelecimento>> GetEstabelecimentosByCategoriaAsync(Guid categoriaId);

        Task<bool> CNPJExisteAsync(string cnpj);

        Task<bool> EmailExisteAsync(string email);

        Task<Estabelecimento?> GetById(Guid id);

        Task<IEnumerable<Estabelecimento>> GetAll();
    }
}