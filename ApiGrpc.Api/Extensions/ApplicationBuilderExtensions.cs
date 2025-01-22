using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.AddCustomer;
using ApiGrpc.Application.Commands.UpdateCustomer;
using ApiGrpc.Application.DTOs;
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
            });

            app.MapGet("/api/customers/{id}", async (Guid id, IMediator mediator) =>
            {
                var query = new GetCustomerByIdQuery(id);
                return await mediator.Send(query);
            });

            app.MapPost("/api/customers", async (CreateCustomerDto dto, IMediator mediator) =>
            {
                var command = new AddCustomerCommand(dto.Name, dto.Email, dto.Phone);
                return await mediator.Send(command);
            });

            app.MapPut("/api/customers/{id}", async (Guid id, UpdateCustomerDto dto, IMediator mediator) =>
            {
                var command = new UpdateCustomerCommand(id, dto.Name, dto.Email, dto.Phone);
                return await mediator.Send(command);
            });

            return app;
        }
    }
}
