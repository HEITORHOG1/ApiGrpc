using ApiGrpc.Application.Commands.Customers.UpdateCustomer;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Customers
{
    public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID � obrigat�rio.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O nome � obrigat�rio.")
                .MaximumLength(100).WithMessage("O nome n�o pode ter mais de 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email � obrigat�rio.")
                .EmailAddress().WithMessage("O email deve ser v�lido.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O telefone � obrigat�rio.")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("O telefone deve ser v�lido.");
        }
    }
}