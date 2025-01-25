using ApiGrpc.Application.Commands.Category;
using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Application.Queries.Category;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class CategoriaGrpcService : CategoriaGrpc.CategoriaGrpcBase
    {
        private readonly IMediator _mediator;

        public CategoriaGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CategoriaResponse> CreateCategoria(CreateCategoriaRequest request, ServerCallContext context)
        {
            var command = new AddCategoriaCommand(
                request.Nome,
                request.Descricao);
            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<CategoriaResponse> UpdateCategoria(UpdateCategoriaRequest request, ServerCallContext context)
        {
            var command = new UpdateCategoriaCommand(Guid.Parse(request.Id), request.Nome, request.Descricao);
            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<CategoriaResponse> GetCategoria(GetCategoriaRequest request, ServerCallContext context)
        {
            var query = new GetCategoriaQuery(Guid.Parse(request.Id));
            var result = await _mediator.Send(query);
            return MapToResponse(result);
        }

        public override async Task<CategoriaListResponse> ListCategorias(ListCategoriasRequest request, ServerCallContext context)
        {
            var query = new GetCategoriasQuery();
            var results = await _mediator.Send(query);

            var response = new CategoriaListResponse();
            response.Categorias.AddRange(results.Select(MapToResponse));
            return response;
        }

        public override async Task<DeleteCategoriaResponse> DeleteCategoria(DeleteCategoriaRequest request, ServerCallContext context)
        {
            var command = new DeleteCategoriaCommand(Guid.Parse(request.Id));
            var result = await _mediator.Send(command);

            return new DeleteCategoriaResponse
            {
                Success = result,
                Id = request.Id 
            };
        }

        private static CategoriaResponse MapToResponse(CategoriaDto dto)
        {
            return new CategoriaResponse
            {
                Id = dto.Id.ToString(),
                Nome = dto.Nome,
                Descricao = dto.Descricao
            };
        }
    }
}