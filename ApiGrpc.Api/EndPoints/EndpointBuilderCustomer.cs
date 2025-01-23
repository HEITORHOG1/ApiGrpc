using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Customers.AddCustomer;
using ApiGrpc.Application.Commands.Customers.UpdateCustomer;
using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Application.Queries.Customers.GetAllCustomers;
using ApiGrpc.Application.Queries.Customers.GetCustomerById;
using MediatR;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderCustomer
    {
        public static WebApplication MapEndpointsCustomer(this WebApplication app)
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
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

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
           .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

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
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

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
           .RequireAuthorization(policy => policy.RequireRole("Admin", "Gerente"));

            return app;
        }
    }
}