using ApiGrpc.Application.DTOs;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand(Guid Id, string Name, string Email, string Phone) : IRequest<CustomerDto>;

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException("Cliente não encontrado");

            customer.Update(request.Name, request.Email, request.Phone);
            await _repository.UpdateAsync(customer);

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}