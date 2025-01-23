using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Address;
using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Application.Queries.Address;
using MediatR;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderEndereco
    {
        public static WebApplication MapEndpointsEndereco(this WebApplication app)
        {
            app.MapGrpcService<EnderecoGrpcService>();

            app.MapPost("/api/enderecos", async (CreateEnderecoDto dto, IMediator mediator) =>
            {
                var command = new AddEnderecoCommand(
                    dto.Logradouro, dto.Numero, dto.Complemento, dto.Bairro,
                    dto.Cidade, dto.Estado, dto.Cep, dto.IsEstabelecimento,
                    dto.UsuarioId, dto.Latitude, dto.Longitude, dto.RaioEntregaKm);
                return await mediator.Send(command);
            })
            .Produces<EnderecoDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("CreateEndereco")
            .WithTags("Enderecos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            app.MapGet("/api/enderecos/{id}", async (Guid id, IMediator mediator) =>
            {
                var query = new GetEnderecoQuery(id);
                return await mediator.Send(query);
            })
            .Produces<EnderecoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetEndereco")
            .WithTags("Enderecos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            app.MapPut("/api/enderecos/{id}", async (Guid id, UpdateEnderecoCommand command, IMediator mediator) =>
            {
                command = command with { Id = id };
                return await mediator.Send(command);
            })
            .Produces<EnderecoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateEndereco")
            .WithTags("Enderecos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            app.MapPut("/api/enderecos/{id}/status", async (Guid id, bool status, IMediator mediator) =>
            {
                var command = new UpdateEnderecoStatusCommand(id, status);
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateEnderecoStatus")
            .WithTags("Enderecos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            app.MapPut("/api/enderecos/{id}/raio-entrega", async (Guid id, double raioKm, IMediator mediator) =>
            {
                var command = new UpdateEnderecoRaioEntregaCommand(id, raioKm);
                await mediator.Send(command);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateEnderecoRaioEntrega")
            .WithTags("Enderecos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            return app;
        }
    }
}