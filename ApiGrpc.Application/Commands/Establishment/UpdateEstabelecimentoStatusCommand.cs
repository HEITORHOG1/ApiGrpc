using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories.Base;
using ApiGrpc.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiGrpc.Application.Commands.Establishment
{
    public record UpdateEstabelecimentoStatusCommand(
         Guid Id,
         bool Status
     ) : IRequest;

    public class UpdateEstabelecimentoStatusCommandHandler
        : IRequestHandler<UpdateEstabelecimentoStatusCommand>
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEstabelecimentoStatusCommandHandler(
            IEstabelecimentoRepository estabelecimentoRepo,
            IUnitOfWork unitOfWork)
        {
            _estabelecimentoRepo = estabelecimentoRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            UpdateEstabelecimentoStatusCommand request,
            CancellationToken cancellationToken)
        {
            // 1. Buscar estabelecimento
            var estabelecimento = await _estabelecimentoRepo.GetByIdAsync(request.Id);
            if (estabelecimento == null)
                throw new NotFoundException("Estabelecimento não encontrado");

            // 2. Atualizar status
            estabelecimento.UpdateStatus(request.Status);

            // 3. Persistir mudanças
            await _estabelecimentoRepo.UpdateAsync(estabelecimento);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
