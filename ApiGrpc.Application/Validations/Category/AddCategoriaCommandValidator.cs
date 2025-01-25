using ApiGrpc.Application.Commands.Category;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Category
{
    public class AddCategoriaCommandValidator : AbstractValidator<AddCategoriaCommand>
    {
        public AddCategoriaCommandValidator()
        {

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(50).WithMessage("Nome deve ter até 50 caracteres");

            RuleFor(x => x.Descricao)
                .MaximumLength(200).WithMessage("Descrição deve ter até 200 caracteres");
        }
    }
}