using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGrpc.Application.Queries.Category
{
    public record GetEstabelecimentosPorCategoriaQuery(Guid CategoriaId) : IRequest<IEnumerable<Guid>>;
}