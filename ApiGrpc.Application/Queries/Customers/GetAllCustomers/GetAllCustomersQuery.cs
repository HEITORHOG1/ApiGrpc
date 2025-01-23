using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Services;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApiGrpc.Application.Queries.Customers.GetAllCustomers
{
    public record GetAllCustomersQuery(int PageNumber = 1, int PageSize = 10)
     : IRequest<IEnumerable<CustomerDto>>;

    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        private readonly CacheService _cacheService;

        public GetAllCustomersQueryHandler(
            ICustomerRepository repository,
            IMapper mapper,
            CacheService cacheService)
        {
            _repository = repository;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            const string cacheKey = "all_customers";
            var cachedCustomers = await _cacheService.GetAsync<IEnumerable<CustomerDto>>(cacheKey);

            if (cachedCustomers is null)
            {
                var data = await _repository.GetAllAsync();
                cachedCustomers = _mapper.Map<IEnumerable<CustomerDto>>(data);

                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                await _cacheService.SetAsync(cacheKey, cachedCustomers, cacheOptions);
            }

            return cachedCustomers;
        }
    }
}