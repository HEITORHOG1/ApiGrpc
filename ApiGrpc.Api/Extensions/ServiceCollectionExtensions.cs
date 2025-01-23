using ApiGrpc.Application.Behaviors;
using ApiGrpc.Application.Commands.Customers.AddCustomer;
using ApiGrpc.Application.Mappings;
using ApiGrpc.Application.Validations.Auth;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories;
using ApiGrpc.Infrastructure.Repositories.Base;
using ApiGrpc.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddGrpc();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddCustomerCommand).Assembly));

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAutoMapper(typeof(EnderecoProfile));

            services.AddScoped<TokenService>();
            services.AddScoped<CacheService>();

            // Registro dos Repositórios
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddDistributedMemoryCache();

            services.AddMemoryCache();
            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("ApiGrpc.Api")
                ));
            return services;
        }
    }
}