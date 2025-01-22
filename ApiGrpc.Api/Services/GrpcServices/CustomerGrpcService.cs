using ApiGrpc.Application.Commands.AddCustomer;
using ApiGrpc.Application.Commands.UpdateCustomer;
using ApiGrpc.Application.DTOs;
using ApiGrpc.Application.Queries.GetAllCustomers;
using ApiGrpc.Application.Queries.GetCustomerById;
using CustomerService.Api;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class CustomerGrpcService : CustomerGrpc.CustomerGrpcBase
    {
        private readonly IMediator _mediator;

        public CustomerGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CustomerResponse> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            var query = new GetCustomerByIdQuery(Guid.Parse(request.Id));
            var result = await _mediator.Send(query);
            return MapToResponse(result);
        }

        public override async Task<CustomerListResponse> GetAllCustomers(GetAllCustomersRequest request, ServerCallContext context)
        {
            var query = new GetAllCustomersQuery();
            var results = await _mediator.Send(query);

            var response = new CustomerListResponse();
            response.Customers.AddRange(results.Select(MapToResponse));
            return response;
        }

        public override async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        {
            var command = new AddCustomerCommand(request.Name, request.Email, request.Phone);
            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<CustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        {
            var command = new UpdateCustomerCommand(
                Guid.Parse(request.Id),
                request.Name,
                request.Email,
                request.Phone);
            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        private static CustomerResponse MapToResponse(CustomerDto dto) =>
            new()
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Active = dto.Active,
                CreatedAt = dto.CreatedAt.ToString("O"),
                UpdatedAt = dto.UpdatedAt?.ToString("O") ?? string.Empty
            };
    }
}
