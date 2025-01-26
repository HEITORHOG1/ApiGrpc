using MediatR;

namespace ApiGrpc.Application.Queries.Category
{
    public record GetEstabelecimentosPorCategoriaQuery(Guid CategoriaId) : IRequest<IEnumerable<Guid>>;
}