using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ApiGrpc.Application.Commands.Auth
{
    public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : IRequest<AuthResponseDto>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration configuration, TokenService tokenService)
        {
            _userManager = userManager;
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

            var (accessToken, refreshToken) = _tokenService.GenerateJwtToken(user);
            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName);
        }
    }
}