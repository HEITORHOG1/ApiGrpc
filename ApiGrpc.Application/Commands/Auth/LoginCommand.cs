using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Infrastructure.Services;
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

        public LoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration, TokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null) throw new NotFoundException("Usuário não encontrado");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded) throw new UnauthorizedAccessException("Credenciais inválidas");

            var (accessToken, refreshToken) = _tokenService.GenerateJwtToken(user);
            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName);
        }
    }
}