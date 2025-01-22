using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Auth;
using ApiGrpc.Application.DTOs.Auth;
using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Application.Queries.Auth;
using MediatR;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderAuth
    {
        public static WebApplication MapEndpointsLogin(this WebApplication app)
        {
            app.MapGrpcService<AuthGrpcService>();

            app.MapPost("/api/auth/register", async (RegisterDto dto, IMediator mediator) =>
            {
                var command = new RegisterCommand(dto.Email, dto.Password, dto.FirstName, dto.LastName);
                return await mediator.Send(command);
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser")
            .WithTags("Auth");

            app.MapPost("/api/auth/login", async (LoginDto dto, IMediator mediator) =>
            {
                var command = new LoginCommand(dto.Email, dto.Password);
                return await mediator.Send(command);
            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("LoginUser")
            .WithTags("Auth");

            app.MapPost("/api/auth/refresh", async (RefreshTokenDto dto, IMediator mediator) =>
            {
                var command = new RefreshTokenCommand(dto.RefreshToken);
                return await mediator.Send(command);
            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RefreshToken")
            .WithTags("Auth");

            app.MapGet("/api/auth/users", async (IMediator mediator) =>
            {
                var query = new GetAllUsersQuery();
                return await mediator.Send(query);
            })
            .Produces<IEnumerable<UserDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetAllUsers")
            .WithTags("Auth");

            return app;
        }
    }
}