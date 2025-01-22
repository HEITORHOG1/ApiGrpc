using ApiGrpc.Api.EndPoints;
using ApiGrpc.Api.Extensions;
using ApiGrpc.Api.Middlewares;
using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Api.Swagger;
using ApiGrpc.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddSwaggerServices()
    .AddPersistence(builder.Configuration)
    .AddIdentityServices()
    .AddAuthenticationServices(builder.Configuration)
    .AddAuthorization()
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
app.UseMiddleware<ValidationExceptionMiddleware>();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Aplica as migra��es pendentes automaticamente
    db.Database.Migrate();
}

app.Run();