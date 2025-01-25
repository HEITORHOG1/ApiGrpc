using ApiGrpc.Application.Commands.Establishment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGrpc.Application.Validations.Establishment
{
    public class UpdateEstabelecimentoValidator : AbstractValidator<UpdateEstabelecimentoCommand>
    {
        public UpdateEstabelecimentoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.RazaoSocial).NotEmpty().MaximumLength(100);
            RuleFor(x => x.CNPJ).NotEmpty().Length(14);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.CategoriaId).NotEmpty();
        }
    }
}
