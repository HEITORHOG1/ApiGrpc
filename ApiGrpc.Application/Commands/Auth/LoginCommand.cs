using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Application.Validations.Auth;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ApiGrpc.Application.Commands.Auth
{
    public record LoginCommand(string Email, string Password) : IRequest<AuthResponseDto>;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly LoginCommandValidator _validationRules;

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            TokenService tokenService, LoginCommandValidator validationRules)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
            _validationRules = validationRules;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            await _validationRules.ValidateAndThrowAsync(request, cancellationToken);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new NotFoundException("Usuário não encontrado");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Credenciais inválidas");

            var (accessToken, refreshToken) = await _tokenService.GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Cliente"; // Assume "Cliente" como padrão se não houver role

            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName, role, user.Id);
        }
    }
}