using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using MediatR;

namespace ApiGrpc.Application.Commands.Address
{
    public record UpdateEnderecoRaioEntregaCommand(Guid Id, double RaioEntregaKm) : IRequest;

    public class UpdateEnderecoRaioEntregaCommandHandler : IRequestHandler<UpdateEnderecoRaioEntregaCommand>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEnderecoRaioEntregaCommandHandler(IEnderecoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateEnderecoRaioEntregaCommand request, CancellationToken cancellationToken)
        {
            var endereco = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Endereço {request.Id} não encontrado");

            endereco.UpdateRaioEntrega(request.RaioEntregaKm);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}