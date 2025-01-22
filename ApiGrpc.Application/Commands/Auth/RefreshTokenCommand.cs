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

        public async Task<AuthResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.ValidateRefreshToken(request.RefreshToken);
            var user = await _userManager.FindByIdAsync(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (user == null) throw new UnauthorizedAccessException("Token inválido");

            var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);

            return new AuthResponseDto(accessToken, refreshToken, user.Email, user.FirstName, user.LastName);
        }
    }
}