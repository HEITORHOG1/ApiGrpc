using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Address
{
    public record GetEnderecosByUsuarioQuery(Guid UsuarioId) : IRequest<IEnumerable<EnderecoDto>>;

    public class GetEnderecosByUsuarioQueryHandler : IRequestHandler<GetEnderecosByUsuarioQuery, IEnumerable<EnderecoDto>>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public GetEnderecosByUsuarioQueryHandler(IEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnderecoDto>> Handle(GetEnderecosByUsuarioQuery request, CancellationToken cancellationToken)
        {
            var enderecos = await _repository.GetByUsuarioAsync(request.UsuarioId);
            return _mapper.Map<IEnumerable<EnderecoDto>>(enderecos);
        }
    }
}