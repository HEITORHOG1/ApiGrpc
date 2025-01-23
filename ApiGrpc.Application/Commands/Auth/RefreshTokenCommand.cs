using ApiGrpc.Application.DTOs.Login;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiGrpc.Application.Commands.Auth
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponseDto>;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponseDto>
    {
        private readonly TokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenCommandHandler(TokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.ValidateRefreshToken(request.RefreshToken);
            var user = await _userManager.FindByIdAsync(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (user == null) throw new UnauthorizedAccessException("Token inválido");

            var (accessToken, refreshToken) = await _tokenService.GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Cliente";

            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName, role, user.Id);
        }
    }
}