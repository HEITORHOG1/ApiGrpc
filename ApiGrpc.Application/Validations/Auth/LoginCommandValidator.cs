using ApiGrpc.Application.Commands.Auth;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Auth
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Digite um email válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo {MinLength} caracteres");
        }
    }
}