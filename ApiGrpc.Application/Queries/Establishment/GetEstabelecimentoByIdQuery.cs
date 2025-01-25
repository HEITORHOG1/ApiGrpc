using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Establishment
{
    public record GetEstabelecimentoByIdQuery(Guid Id) : IRequest<EstabelecimentoDto>;

    public class GetEstabelecimentoByIdQueryHandler : IRequestHandler<GetEstabelecimentoByIdQuery, EstabelecimentoDto>
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepo;
        private readonly IMapper _mapper;

        public GetEstabelecimentoByIdQueryHandler(
            IEstabelecimentoRepository estabelecimentoRepo,
            IMapper mapper)
        {
            _estabelecimentoRepo = estabelecimentoRepo;
            _mapper = mapper;
        }

        public async Task<EstabelecimentoDto> Handle(GetEstabelecimentoByIdQuery request, CancellationToken cancellationToken)
        {
            var estabelecimento = await _estabelecimentoRepo.GetByIdAsync(request.Id);
            if (estabelecimento == null)
                throw new NotFoundException("Estabelecimento não encontrado");

            return _mapper.Map<EstabelecimentoDto>(estabelecimento);
        }
    }
}