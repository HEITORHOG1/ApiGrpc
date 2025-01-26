using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using MediatR;

namespace ApiGrpc.Application.Commands.Address
{
    public class UpdateEnderecoStatusCommand : IRequest
    {
        public Guid Id { get; }
        public bool Status { get; }

        public UpdateEnderecoStatusCommand(Guid id, bool status)
        {
            Id = id;
            Status = status;
        }
    }

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
                ?? throw new NotFoundException($"Endereço {request.Id} não encontrado");

            endereco.UpdateStatus(request.Status);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}