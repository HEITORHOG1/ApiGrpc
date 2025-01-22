using ApiGrpc.Api.Services.GrpcServices;
using ApiGrpc.Application.Commands.Auth;
using ApiGrpc.Application.DTOs.Auth;
using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Application.Queries.Auth;
using ApiGrpc.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiGrpc.Api.EndPoints
{
    public static class EndpointBuilderAuth
    {
        public static WebApplication MapEndpointsLogin(this WebApplication app)
        {
            app.MapGrpcService<AuthGrpcService>();

            app.MapPost("/api/auth/register", async (RegisterDto dto, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) =>
            {
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, dto.Password);
                if (!new[] { "Admin", "Cliente", "Gerente" }.Contains(dto.Role))
                {
                    return Results.BadRequest(new { Error = "Role inválido" });
                }
                if (result.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync(dto.Role))
                    {
                        return Results.BadRequest(new { Error = "Role não existe." });
                    }

                    await userManager.AddToRoleAsync(user, dto.Role);
                    return Results.Created($"/api/auth/users/{user.Id}", user);
                }

                return Results.BadRequest(result.Errors);
            })
             .Produces(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .WithName("RegisterUser")
             .WithTags("Auth");

            app.MapPost("/api/auth/login", async (LoginDto dto, IMediator mediator) =>
            {
                var command = new LoginCommand(dto.Email, dto.Password);
                return await mediator.Send(command);
            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("LoginUser")
            .WithTags("Auth");

            app.MapPost("/api/auth/refresh", async (RefreshTokenDto dto, IMediator mediator) =>
            {
                var command = new RefreshTokenCommand(dto.RefreshToken);
                return await mediator.Send(command);
            })
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("RefreshToken")
            .WithTags("Auth");

            app.MapGet("/api/auth/users", async (IMediator mediator) =>
            {
                var query = new GetAllUsersQuery();
                return await mediator.Send(query);
            })
            .Produces<IEnumerable<UserDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetAllUsers")
            .WithTags("Auth")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Cliente", "Gerente"));

            app.MapGet("/api/auth/roles", async (RoleManager<IdentityRole> roleManager) =>
            {
                var roles = await roleManager.Roles.ToListAsync();
                return Results.Ok(roles);
            })
            .Produces<IEnumerable<IdentityRole>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("GetAllRoles")
            .WithTags("Auth");

            app.MapPut("/api/auth/users/{id}", async (Guid id, UpdateUserDto dto, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) =>
            {
                var user = await userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return Results.NotFound(new { Error = "Usuário não encontrado." });
                }

                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.FirstName = dto.FirstName;
                user.LastName = dto.LastName;

                var result = await userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Results.BadRequest(result.Errors);
                }

                // Atualizar role do usuário
                var currentRoles = await userManager.GetRolesAsync(user);
                await userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!await roleManager.RoleExistsAsync(dto.Role))
                {
                    return Results.BadRequest(new { Error = "Role não existe." });
                }
                await userManager.AddToRoleAsync(user, dto.Role);

                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("UpdateUser")
            .WithTags("Auth")
            .RequireAuthorization(policy => policy.RequireRole("Admin", "Cliente", "Gerente"));

            return app;
        }
    }
}