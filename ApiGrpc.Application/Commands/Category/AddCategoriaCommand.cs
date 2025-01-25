using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.Category
{
    // Create Command
    public record AddCategoriaCommand(
                                 string Nome,
                                 string Descricao) : IRequest<CategoriaDto>;

    public class AddCategoriaCommandHandler : IRequestHandler<AddCategoriaCommand, CategoriaDto>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCategoriaCommandHandler(
            ICategoriaRepository repository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoriaDto> Handle(AddCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = new Categoria(request.Nome, request.Descricao);

            await _repository.AddAsync(categoria);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoriaDto>(categoria);
        }
    }
}