using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Queries.Category
{
    public record GetCategoriasByEstabelecimentoQuery(Guid EstabelecimentoId)
     : IRequest<IEnumerable<CategoriaDto>>;

    public class GetCategoriasByEstabelecimentoQueryHandler
    : IRequestHandler<GetCategoriasByEstabelecimentoQuery, IEnumerable<CategoriaDto>>
    {
        private readonly ICategoriaRepository _repository;

        private readonly IMapper _mapper;

        public GetCategoriasByEstabelecimentoQueryHandler(ICategoriaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoriaDto>> Handle(GetCategoriasByEstabelecimentoQuery request, CancellationToken cancellationToken)
        {
            var categorias = await _repository.GetByEstabelecimentoAsync();
            return _mapper.Map<IEnumerable<CategoriaDto>>(categorias);
        }
    }
}