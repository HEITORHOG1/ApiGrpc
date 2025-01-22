using ApiGrpc.Api.Extensions;
using ApiGrpc.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddApplicationServices()
    .AddSwaggerServices()
    .AddPersistence(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwaggerEndpoints()
   .UseRouting();

app.MapEndpoints();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    db.Database.Migrate();
}

app.Run();