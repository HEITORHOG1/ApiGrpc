using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Application.Validations.Customers;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace ApiGrpc.Application.Commands.Customers.UpdateCustomer
{
    public record UpdateCustomerCommand(Guid Id, string Name, string Email, string Phone) : IRequest<CustomerDto>;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UpdateCustomerCommandValidator _validationRules;

        public UpdateCustomerCommandHandler(
            ICustomerRepository repository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            UpdateCustomerCommandValidator validationRules)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validationRules = validationRules;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            await _validationRules.ValidateAndThrowAsync(request, cancellationToken);
            var customer = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Cliente não encontrado");

            customer.Update(request.Name, request.Email, request.Phone);
            await _repository.UpdateAsync(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}