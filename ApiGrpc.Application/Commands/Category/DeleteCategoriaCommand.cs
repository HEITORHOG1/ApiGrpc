using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using MediatR;

namespace ApiGrpc.Application.Commands.Category
{
    // Delete Command
    public record DeleteCategoriaCommand(Guid Id) : IRequest<bool>;

    public class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, bool>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoriaCommandHandler(
            ICategoriaRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);
            if (categoria == null)
                return false;

            await _repository.Remove(categoria);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}