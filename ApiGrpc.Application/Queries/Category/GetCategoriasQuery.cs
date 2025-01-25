using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Category
{
    // Get All
    public record GetCategoriasQuery() : IRequest<IEnumerable<CategoriaDto>>;

    public class GetCategoriasQueryHandler : IRequestHandler<GetCategoriasQuery, IEnumerable<CategoriaDto>>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IMapper _mapper;

        public GetCategoriasQueryHandler(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> Handle(GetCategoriasQuery request, CancellationToken cancellationToken)
        {
            var categorias = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }
    }
}