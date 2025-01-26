using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Address
{
    public record GetEnderecoQuery(Guid Id) : IRequest<EnderecoDto>;

    public class GetEnderecoQueryHandler : IRequestHandler<GetEnderecoQuery, EnderecoDto>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IMapper _mapper;

        public GetEnderecoQueryHandler(IEnderecoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<EnderecoDto> Handle(GetEnderecoQuery request, CancellationToken cancellationToken)
        {
            var endereco = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Endereço {request.Id} não encontrado");

            return _mapper.Map<EnderecoDto>(endereco);
        }
    }
}