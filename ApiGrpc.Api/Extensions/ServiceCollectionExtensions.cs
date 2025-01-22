using ApiGrpc.Application.Behaviors;
using ApiGrpc.Application.Commands.Customers.AddCustomer;
using ApiGrpc.Application.Mappings;
using ApiGrpc.Application.Validations.Auth;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Infrastructure.Context;
using ApiGrpc.Infrastructure.Repositories;
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
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddValidatorsFromAssembly(typeof(LoginCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

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