using ApiGrpc.Application.DTOs.Establishment;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Establishment
{
    public class CreateEstabelecimentoValidator : AbstractValidator<CreateEstabelecimentoDto>
    {
        public CreateEstabelecimentoValidator()
        {
            RuleFor(x => x.UsuarioId).NotEmpty();
            RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(100);
            RuleFor(x => x.NomeFantasia).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Telefone).NotEmpty().Matches(@"^\d{10,11}$");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500);
            RuleFor(x => x.CategoriaId).NotEmpty();
        }
    }
}