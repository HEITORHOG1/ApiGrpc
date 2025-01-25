using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGrpc.Application.Queries.Establishment
{
    public record GetEstabelecimentosByUsuarioQuery(
       Guid UsuarioId
   ) : IRequest<IEnumerable<EstabelecimentoDto>>;

    public class GetEstabelecimentosByUsuarioQueryHandler
        : IRequestHandler<GetEstabelecimentosByUsuarioQuery, IEnumerable<EstabelecimentoDto>>
    {
        private readonly IEstabelecimentoRepository _repo;
        private readonly IMapper _mapper;

        public GetEstabelecimentosByUsuarioQueryHandler(
            IEstabelecimentoRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstabelecimentoDto>> Handle(
            GetEstabelecimentosByUsuarioQuery request,
            CancellationToken cancellationToken)
        {
            var estabelecimentos = await _repo.GetEstabelecimentosByUsuarioAsync(request.UsuarioId);
            return _mapper.Map<IEnumerable<EstabelecimentoDto>>(estabelecimentos);
        }
    }
}
