using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiGrpc.Application.Queries.Customers.GetCustomerById
{
    public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;

    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly CacheService _cacheService;

        public GetCustomerByIdQueryHandler(
            ICustomerRepository repository,
            IMapper mapper,
            CacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<CustomerDto> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"customer_{request.Id}";
            var cachedCustomer = await _cacheService.GetAsync<CustomerDto>(cacheKey);

            if (cachedCustomer is null)
            {
                var customer = await _repository.GetByIdAsync(request.Id)
                    ?? throw new NotFoundException("Cliente não encontrado");

                cachedCustomer = _mapper.Map<CustomerDto>(customer);

                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                await _cacheService.SetAsync(cacheKey, cachedCustomer, cacheOptions);
            }

            return cachedCustomer;
        }
    }
}