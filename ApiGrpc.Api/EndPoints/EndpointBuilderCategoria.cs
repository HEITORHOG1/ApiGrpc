using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Category;
using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Application.Queries.Category;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderCategoria
    {
        public static WebApplication MapEndpointsCategoria(this WebApplication app)
        {
            app.MapGrpcService<CategoriaGrpcService>();

            app.MapPost("/api/categorias", async ([FromBody] CreateCategoriaDto dto, IMediator mediator) =>
            {
                var command = new AddCategoriaCommand(dto.Nome, dto.Descricao);
                return await mediator.Send(command);
            })
            .Produces<CategoriaDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateCategoria")
            .WithTags("Categorias")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

            app.MapGet("/api/categorias/{id}", async (Guid id, IMediator mediator) =>
            {
                var query = new GetCategoriaQuery(id);
                return await mediator.Send(query);
            })
            .Produces<CategoriaDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("GetCategoria")
            .WithTags("Categorias");

            app.MapPut("/api/categorias/{id}", async (Guid id, [FromBody] UpdateCategoriaCommand command, IMediator mediator) =>
            {
                command = command with { Id = id };
                return await mediator.Send(command);
            })
            .Produces<CategoriaDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateCategoria")
            .WithTags("Categorias")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

            app.MapGet("/api/categorias", async (IMediator mediator) =>
            {
                var query = new GetCategoriasQuery();
                return await mediator.Send(query);
            })
            .Produces<IEnumerable<CategoriaDto>>(StatusCodes.Status200OK)
            .WithName("GetAllCategorias")
            .WithTags("Categorias");

            app.MapGet("/api/estabelecimentos/{estabelecimentoId}/categorias",
            async (Guid estabelecimentoId, IMediator mediator) =>
            {
                var query = new GetCategoriasByEstabelecimentoQuery(estabelecimentoId);
                return await mediator.Send(query);
            })
             .Produces<IEnumerable<CategoriaDto>>(StatusCodes.Status200OK)
             .WithName("GetCategoriasByEstabelecimento")
             .WithTags("Categorias");

            app.MapDelete("/api/categorias/{id}", async (Guid id, IMediator mediator) =>
            {
                var command = new DeleteCategoriaCommand(id);
                return await mediator.Send(command);
            }).Produces(StatusCodes.Status204NoContent).Produces(StatusCodes.Status404NotFound)
            .WithName("DeleteCategoria")
            .WithTags("Categorias");

            return app;
        }
    }
}