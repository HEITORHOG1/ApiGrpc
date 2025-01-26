using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.Address
{
    public record UpdateEnderecoCommand(
    Guid Id,
    string Logradouro,
    string Numero,
    string Complemento,
    string Bairro,
    string Cidade,
    string Estado,
    string Cep,
    double Latitude,
    double Longitude) : IRequest<EnderecoDto>;

    public class UpdateEnderecoCommandHandler : IRequestHandler<UpdateEnderecoCommand, EnderecoDto>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEnderecoCommandHandler(
            IEnderecoRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EnderecoDto> Handle(UpdateEnderecoCommand request, CancellationToken cancellationToken)
        {
            var endereco = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Endereço {request.Id} não encontrado");

            endereco.Update(
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.Latitude,
                request.Longitude);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<EnderecoDto>(endereco);
        }
    }
}