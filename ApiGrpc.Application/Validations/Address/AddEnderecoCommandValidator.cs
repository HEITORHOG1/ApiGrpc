using ApiGrpc.Application.Commands.Address;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Address
{
    public class AddEnderecoCommandValidator : AbstractValidator<AddEnderecoCommand>
    {
        public AddEnderecoCommandValidator()
        {
            RuleFor(x => x.Logradouro).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Numero).NotEmpty().MaximumLength(10);
            RuleFor(x => x.Bairro).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Cidade).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Estado).NotEmpty().Length(2);
            RuleFor(x => x.Cep).NotEmpty().Length(8);
            RuleFor(x => x.UsuarioId).NotEmpty();
            RuleFor(x => x.RaioEntregaKm).GreaterThan(0).When(x => x.IsEstabelecimento);
        }
    }
}