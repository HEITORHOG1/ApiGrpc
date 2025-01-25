using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Category
{
    // Get by Id
    public record GetCategoriaQuery(Guid Id) : IRequest<CategoriaDto>;

    public class GetCategoriaQueryHandler : IRequestHandler<GetCategoriaQuery, CategoriaDto>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public GetCategoriaQueryHandler(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CategoriaDto> Handle(GetCategoriaQuery request, CancellationToken cancellationToken)
        {
            var categoria = await _repository.GetByIdAsync(request.Id);
            return _mapper.Map<CategoriaDto>(categoria);
        }
    }
}