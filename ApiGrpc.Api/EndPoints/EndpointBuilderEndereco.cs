using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Address;
using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Application.Queries.Address;
using ApiGrpc.Domain.Exceptions;
using FluentValidation;
using MediatR;
using System.Security.Claims;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderEndereco
    {
        public static WebApplication MapEndpointsEndereco(this WebApplication app)
        {
            app.MapGrpcService<EnderecoGrpcService>();

            var group = app.MapGroup("/api/enderecos")
                .WithTags("Enderecos")
                .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente", "Proprietario"));

            group.MapPost("", CreateEndereco)
                .Produces<EnderecoDto>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .WithName("CreateEndereco");

            group.MapGet("{id}", GetEndereco)
                .Produces<EnderecoDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetEndereco");

            group.MapPut("{id}", UpdateEndereco)
                .Produces<EnderecoDto>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateEndereco");

            group.MapPut("{id}/status", UpdateEnderecoStatus)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateEnderecoStatus");

            group.MapPut("{id}/raio-entrega", UpdateEnderecoRaioEntrega)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("UpdateEnderecoRaioEntrega");

            return app;
        }

        private static async Task<IResult> CreateEndereco(
            CreateEnderecoDto dto,
            IMediator mediator,
            HttpContext context)
        {
            try
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                    return Results.Unauthorized();

                var userId = Guid.Parse(userIdClaim);

                if (dto.IsEstabelecimento && !dto.EstabelecimentoId.HasValue)
                {
                    return Results.BadRequest("EstabelecimentoId é obrigatório para endereços de estabelecimento");
                }

                if (!dto.IsEstabelecimento)
                {
                    dto = dto with { UsuarioId = userId };
                }

                var command = new AddEnderecoCommand(
                    dto.Logradouro,
                    dto.Numero,
                    dto.Complemento,
                    dto.Bairro,
                    dto.Cidade,
                    dto.Estado,
                    dto.Cep,
                    dto.IsEstabelecimento,
                    dto.UsuarioId,
                    dto.EstabelecimentoId,
                    dto.Latitude,
                    dto.Longitude,
                    dto.RaioEntregaKm);

                var result = await mediator.Send(command);
                return Results.Created($"/api/enderecos/{result.Id}", result);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
            catch (DomainException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> GetEndereco(
            Guid id,
            IMediator mediator)
        {
            try
            {
                var query = new GetEnderecoQuery(id);
                var result = await mediator.Send(query);
                return Results.Ok(result);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        private static async Task<IResult> UpdateEndereco(
            Guid id,
            UpdateEnderecoCommand command,
            IMediator mediator)
        {
            try
            {
                command = command with { Id = id };
                var result = await mediator.Send(command);
                return Results.Ok(result);
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return Results.BadRequest(ex.Errors);
            }
            catch (DomainException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        private static async Task<IResult> UpdateEnderecoStatus(
            Guid id,
            bool status,
            IMediator mediator)
        {
            try
            {
                var command = new UpdateEnderecoStatusCommand(id, status);
                await mediator.Send(command);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
        }

        private static async Task<IResult> UpdateEnderecoRaioEntrega(
            Guid id,
            double raioKm,
            IMediator mediator)
        {
            try
            {
                var command = new UpdateEnderecoRaioEntregaCommand(id, raioKm);
                await mediator.Send(command);
                return Results.NoContent();
            }
            catch (NotFoundException ex)
            {
                return Results.NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}