using ApiGrpc.Application.DTOs;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.AddCustomer
{
    public record AddCustomerCommand(string Name, string Email, string Phone) : IRequest<CustomerDto>;

    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public AddCustomerCommandHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.EmailExistsAsync(request.Email))
                throw new DomainException("Email já cadastrado");

            var customer = new Customer(request.Name, request.Email, request.Phone);
            await _repository.AddAsync(customer);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}