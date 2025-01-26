using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Application.Validations.Customers;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace ApiGrpc.Application.Commands.Customers.AddCustomer
{
    public record AddCustomerCommand(string Name, string Email, string Phone) : IRequest<CustomerDto>;

    public sealed class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly AddCustomerCommandValidator validationRules;

        public AddCustomerCommandHandler(
            ICustomerRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            AddCustomerCommandValidator validationRules)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            this.validationRules = validationRules;
        }

        public async Task<CustomerDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            await validationRules.ValidateAndThrowAsync(request, cancellationToken);
            if (await _repository.EmailExistsAsync(request.Email))
                throw new DomainException("Email já cadastrado");

            var customer = new Customer(request.Name, request.Email, request.Phone);
            await _repository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CustomerDto>(customer);
        }
    }
}