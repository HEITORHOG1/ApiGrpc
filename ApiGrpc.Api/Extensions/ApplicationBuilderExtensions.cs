using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.AddCustomer;
using ApiGrpc.Application.Commands.Auth;
using ApiGrpc.Application.Commands.UpdateCustomer;
using ApiGrpc.Application.DTOs.Auth;
using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Application.Queries.Auth;
using ApiGrpc.Application.Queries.GetAllCustomers;
using ApiGrpc.Application.Queries.GetCustomerById;
using MediatR;

namespace ApiGrpc.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static WebApplication UseSwaggerEndpoints(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer gRPC API V1");
                c.RoutePrefix = "swagger";
            });
            return app;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            app.MapGrpcService<CustomerGrpcService>();
            app.MapGet("/", () => Results.Redirect("/swagger"));

            app.MapGet("/api/customers", async (IMediator mediator) =>
            {
                var query = new GetAllCustomersQuery();
                return await mediator.Send(query);
            })
            .Produces<IEnumerable<CustomerDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetAllCustomers")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapGet("/api/customers/{id}", async (Guid id, IMediator mediator) =>
            {
                var query = new GetCustomerByIdQuery(id);
                return await mediator.Send(query);
            })
            .Produces<CustomerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetCustomerById")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPost("/api/customers", async (CreateCustomerDto dto, IMediator mediator) =>
            {
                var command = new AddCustomerCommand(dto.Name, dto.Email, dto.Phone);
                return await mediator.Send(command);
            })
            .Produces<CustomerDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("AddCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            app.MapPut("/api/customers/{id}", async (Guid id, UpdateCustomerDto dto, IMediator mediator) =>
            {
                var command = new UpdateCustomerCommand(id, dto.Name, dto.Email, dto.Phone);
                return await mediator.Send(command);
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("UpdateCustomer")
            .WithTags("Customers")
            .RequireAuthorization();

            return app;
        }

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
