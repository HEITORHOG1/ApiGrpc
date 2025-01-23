using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using MediatR;

namespace ApiGrpc.Application.Commands.Address
{
    public record UpdateEnderecoStatusCommand(Guid Id, bool Status) : IRequest;

    public class UpdateEnderecoStatusCommandHandler : IRequestHandler<UpdateEnderecoStatusCommand>
    {
        private readonly IEnderecoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEnderecoStatusCommandHandler(IEnderecoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateEnderecoStatusCommand request, CancellationToken cancellationToken)
        {
            var endereco = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Endereço não encontrado");

            endereco.UpdateStatus(request.Status);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}