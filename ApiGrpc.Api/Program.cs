using ApiGrpc.Api.EndPoints;
using ApiGrpc.Api.Extensions;
using ApiGrpc.Api.Middlewares;
using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Api.Swagger;
using ApiGrpc.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddApplicationServices()
    .AddSwaggerServices()
    .AddPersistence(builder.Configuration)
    .AddIdentityServices()
    .AddAuthenticationServices(builder.Configuration)
    .AddAuthorization(options =>
    {
        options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        options.AddPolicy("RequireManagerRole", policy => policy.RequireRole("Gerente"));
        options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Cliente"));
    })
    .AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerEndpoints();
}

app.UseRouting()
   .UseAuthentication()
   .UseAuthorization()
   .UseGrpcWeb();

app.MapGrpcService<AuthGrpcService>().EnableGrpcWeb().RequireAuthorization();
app.MapGrpcService<CustomerGrpcService>().EnableGrpcWeb().RequireAuthorization();
app.MapEndpointsCustomer();
app.MapEndpointsLogin();
app.MapEndpointsEndereco();
app.MapEndpointsEstabelecimento();
app.MapEndpointsCategoria();
app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    // Aplica as migrações pendentes automaticamente
    db.Database.Migrate();
    await CreateRoles(services);
}

app.Run();

async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "Cliente", "Gerente" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}