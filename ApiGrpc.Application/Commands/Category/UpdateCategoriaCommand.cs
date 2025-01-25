using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.Category
{
    // Update Command
    public record UpdateCategoriaCommand(
                                 Guid Id,
                                 string Nome,
                                 string Descricao) : IRequest<CategoriaDto>;

    public class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, CategoriaDto>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoriaCommandHandler(
            ICategoriaRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoriaDto> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);
            categoria.Update(request.Nome, request.Descricao);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CategoriaDto>(categoria);
        }
    }
}