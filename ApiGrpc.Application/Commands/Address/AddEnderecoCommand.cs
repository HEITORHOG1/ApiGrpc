using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.Address
{
    public record AddEnderecoCommand(
        string Logradouro,
        string Numero,
        string Complemento,
        string Bairro,
        string Cidade,
        string Estado,
        string Cep,
        bool IsEstabelecimento,
        Guid? UsuarioId,
        Guid? EstabelecimentoId,
        double Latitude,
        double Longitude,
        double? RaioEntregaKm
    ) : IRequest<EnderecoDto>;

    public class AddEnderecoCommandHandler : IRequestHandler<AddEnderecoCommand, EnderecoDto>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddEnderecoCommandHandler(
            IEnderecoRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnderecoDto> Handle(AddEnderecoCommand request, CancellationToken cancellationToken)
        {
            var endereco = new Endereco(
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.IsEstabelecimento,
                request.UsuarioId,
                request.EstabelecimentoId,
                request.Latitude,
                request.Longitude,
                request.RaioEntregaKm);

            await _repository.AddAsync(endereco);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EnderecoDto>(endereco);
        }
    }
}