using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Establishment;
using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Application.Queries.Establishment;
using MediatR;
using System.Security.Claims;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderEstabelecimento
    {
        public static WebApplication MapEndpointsEstabelecimento(this WebApplication app)
        {
            app.MapGrpcService<EstabelecimentoGrpcService>();

            // POST
            app.MapPost("/api/estabelecimentos", async (
                                                        CreateEstabelecimentoDto dto,
                                                        IMediator mediator,
                                                        HttpContext httpContext
                                                    ) =>
            {
                var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Results.Unauthorized();
                }

                var userId = Guid.Parse(userIdClaim);

                var command = new AddEstabelecimentoCommand(
                    userId,
                    dto.RazaoSocial,
                    dto.NomeFantasia,
                    dto.CNPJ,
                    dto.Telefone,
                    dto.Email,
                    dto.UrlImagem,
                    dto.Descricao,
                    dto.InscricaoEstadual,
                    dto.InscricaoMunicipal,
                    dto.Website,
                    dto.RedeSocial,
                    dto.CategoriaId
                );

                var result = await mediator.Send(command);

                return Results.Created($"/api/estabelecimentos/{result.Id}", result);
            })
            .Produces<EstabelecimentoDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("CreateEstabelecimento")
            .WithTags("Estabelecimentos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente", "Proprietario"));

            // GET
            app.MapGet("/api/estabelecimentos/{id}", async (Guid id, IMediator mediator) =>
            {
                // O handler pode lançar NotFoundException.
                var query = new GetEstabelecimentoByIdQuery(id);
                var estabelecimento = await mediator.Send(query);
                return estabelecimento is null
                    ? Results.NotFound()
                    : Results.Ok(estabelecimento);
            })
            .WithName("GetEstabelecimento")
            .WithTags("Estabelecimentos");

            // PUT
            app.MapPut("/api/estabelecimentos/{id}", async (Guid id, UpdateEstabelecimentoCommand command, IMediator mediator) =>
            {
                // Garante que o Id do command seja o mesmo da rota
                command = command with { Id = id };

                var result = await mediator.Send(command);
                return Results.Ok(result);
            })
            .WithName("UpdateEstabelecimento")
            .WithTags("Estabelecimentos")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente", "Proprietario"));

            // PUT - Status
            app.MapPut("/api/estabelecimentos/{id}/status", async (Guid id, bool status, IMediator mediator) =>
            {
                var command = new UpdateEstabelecimentoStatusCommand(id, status);
                await mediator.Send(command);
                return Results.NoContent();
            })
            .WithName("UpdateEstabelecimentoStatus")
            .WithTags("Estabelecimentos")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

            // get all estabelecimentos
            app.MapGet("/api/estabelecimentos", async (IMediator mediator) =>
            {
                var query = new GetAllEstabelecimentosQuery();
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
                .Produces<IEnumerable<EstabelecimentoDto>>(StatusCodes.Status200OK)
                .WithName("GetAllEstabelecimentos")
                .WithTags("Estabelecimentos");

            return app;
        }
    }
}