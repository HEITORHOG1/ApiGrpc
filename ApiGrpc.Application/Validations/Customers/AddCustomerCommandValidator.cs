using FluentValidation;

namespace ApiGrpc.Application.Commands.Customers.AddCustomer
{
    public class AddCustomerCommandValidator : AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerCommandValidator()
        {
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