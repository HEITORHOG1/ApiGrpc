using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Establishment
{
    public record GetEnderecosByEstabelecimentoQuery(Guid EstabelecimentoId) : IRequest<IEnumerable<EnderecoDto>>;

    public class GetEnderecosByEstabelecimentoQueryHandler : IRequestHandler<GetEnderecosByEstabelecimentoQuery, IEnumerable<EnderecoDto>>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public GetEnderecosByEstabelecimentoQueryHandler(IEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnderecoDto>> Handle(GetEnderecosByEstabelecimentoQuery request, CancellationToken cancellationToken)
        {
            var enderecos = await _repository.GetEstabelecimentosAsync();

            var estabelecimentoEnderecos = enderecos.Where(e =>
                e.EstabelecimentoId == request.EstabelecimentoId);

            return _mapper.Map<IEnumerable<EnderecoDto>>(estabelecimentoEnderecos);
        }
    }
}