using ApiGrpc.Application.DTOs.Auth;
using ApiGrpc.Application.Validations.Auth;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Infrastructure.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ApiGrpc.Application.Commands.Auth
{
    public record RegisterCommand(
         string Email,
         string Password,
         string FirstName,
         string LastName,
         string Role
     ) : IRequest<AuthResponseDto>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly TokenService _tokenService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private readonly RegisterCommandValidator _validator;

        public RegisterCommandHandler(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            TokenService tokenService,
            ILogger<RegisterCommandHandler> logger,
            RegisterCommandValidator validator)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // 1. Validação dos dados de entrada
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            // 2. Verificar e-mail único
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Tentativa de registro com e-mail já existente: {Email}", request.Email);
                throw new ConflictException("Este e-mail já está cadastrado");
            }

            // 3. Criar novo usuário
            var user = new ApplicationUser(request.Email, request.FirstName, request.LastName);
            var createResult = await _userManager.CreateAsync(user, request.Password);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                _logger.LogError("Falha ao criar usuário: {Errors}", errors);
                throw new ValidationException($"Falha no registro: {errors}");
            }

            // 4. Verificar e atribuir role
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                _logger.LogError("Tentativa de atribuir role inexistente: {Role}", request.Role);
                throw new NotFoundException("Perfil de acesso não encontrado");
            }

            var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
            if (!roleResult.Succeeded)
            {
                var errors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                _logger.LogError("Falha ao atribuir role: {Errors}", errors);
                throw new ApplicationException("Falha ao configurar perfil de acesso");
            }

            // 5. Gerar tokens
            var tokens = await _tokenService.GenerateJwtToken(user);

            _logger.LogInformation("Novo usuário registrado com sucesso: {Email}", request.Email);

            return new AuthResponseDto(
                tokens.accessToken,
                tokens.refreshToken,
                user.Email,
                user.FirstName,
                user.LastName,
                request.Role,
                user.Id.ToString());
        }
    }
}