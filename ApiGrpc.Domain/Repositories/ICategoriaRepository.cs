using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories.Base;

namespace ApiGrpc.Domain.Repositories
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<bool> ExisteCategoriaPorNomeAsync(string nome);

        Task<IEnumerable<Categoria>> GetByEstabelecimentoAsync();
    }
}