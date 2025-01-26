using ApiGrpc.Application.Commands.Address;
using FluentValidation;

namespace ApiGrpc.Application.Validations.Address
{
    public class AddEnderecoCommandValidator : AbstractValidator<AddEnderecoCommand>
    {
        public AddEnderecoCommandValidator()
        {
            RuleFor(x => x.Logradouro)
                .NotEmpty().WithMessage("Logradouro é obrigatório")
                .MaximumLength(100).WithMessage("Logradouro deve ter no máximo 100 caracteres");

            RuleFor(x => x.Numero)
                .NotEmpty().WithMessage("Número é obrigatório")
                .MaximumLength(10).WithMessage("Número deve ter no máximo 10 caracteres");

            RuleFor(x => x.Complemento)
                .MaximumLength(50).WithMessage("Complemento deve ter no máximo 50 caracteres");

            RuleFor(x => x.Bairro)
                .NotEmpty().WithMessage("Bairro é obrigatório")
                .MaximumLength(50).WithMessage("Bairro deve ter no máximo 50 caracteres");

            RuleFor(x => x.Cidade)
                .NotEmpty().WithMessage("Cidade é obrigatória")
                .MaximumLength(50).WithMessage("Cidade deve ter no máximo 50 caracteres");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("Estado é obrigatório")
                .Length(2).WithMessage("Estado deve ter 2 caracteres");

            RuleFor(x => x.Cep)
                .NotEmpty().WithMessage("CEP é obrigatório")
                .Length(8).WithMessage("CEP deve ter 8 caracteres")
                .Matches(@"^\d{8}$").WithMessage("CEP deve conter apenas números");

            RuleFor(x => x.EstabelecimentoId)
                .NotEmpty().When(x => x.IsEstabelecimento)
                .WithMessage("EstabelecimentoId é obrigatório para endereços de estabelecimento");

            RuleFor(x => x.UsuarioId)
                .NotEmpty().Unless(x => x.IsEstabelecimento)
                .WithMessage("UsuarioId é obrigatório para endereços de usuário");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90)
                .WithMessage("Latitude deve estar entre -90 e 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180)
                .WithMessage("Longitude deve estar entre -180 e 180");

            RuleFor(x => x.RaioEntregaKm)
                .GreaterThan(0).When(x => x.IsEstabelecimento)
                .WithMessage("Raio de entrega deve ser maior que 0 para estabelecimentos");
        }
    }
}