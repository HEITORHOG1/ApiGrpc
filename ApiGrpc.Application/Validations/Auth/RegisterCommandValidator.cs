using ApiGrpc.Application.Commands.Auth;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Auth
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Digite um email válido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(8).WithMessage("A senha deve ter no mínimo {MinLength} caracteres");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("O nome é obrigatório")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo {MaxLength} caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("O sobrenome é obrigatório")
                .MaximumLength(100).WithMessage("O sobrenome deve ter no máximo {MaxLength} caracteres");
        }
    }
}