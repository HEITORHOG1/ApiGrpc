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
    public record GetAllEstabelecimentosQuery : IRequest<IEnumerable<EstabelecimentoDto>>;

    public class GetAllEstabelecimentosQueryHandler : IRequestHandler<GetAllEstabelecimentosQuery, IEnumerable<EstabelecimentoDto>>
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepo;
        private readonly IMapper _mapper;

        public GetAllEstabelecimentosQueryHandler(
            IEstabelecimentoRepository estabelecimentoRepo,
            IMapper mapper)
        {
            _estabelecimentoRepo = estabelecimentoRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EstabelecimentoDto>> Handle(GetAllEstabelecimentosQuery request, CancellationToken cancellationToken)
        {
            var estabelecimentos = await _estabelecimentoRepo.GetAll();
            return _mapper.Map<IEnumerable<EstabelecimentoDto>>(estabelecimentos);
        }
    }
}
