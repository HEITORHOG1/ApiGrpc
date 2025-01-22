using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ApiGrpc.Application.Commands.Auth
{
    public record RegisterCommand(string Email, string Password, string FirstName, string LastName, string Role) : IRequest<AuthResponseDto>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, TokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                throw new DomainException("Email já registrado");

            var user = new ApplicationUser(request.Email, request.FirstName, request.LastName);
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new DomainException(result.Errors.First().Description);

            // Verificar se o role existeLoginCommandHandler
            if (!await _roleManager.RoleExistsAsync(request.Role))
                throw new DomainException("Role não existe");

            // Atribuir role ao usuário
            await _userManager.AddToRoleAsync(user, request.Role);

            var (accessToken, refreshToken) = await _tokenService.GenerateJwtToken(user);
            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName, request.Role);
        }
    }
}