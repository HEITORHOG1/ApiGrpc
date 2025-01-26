using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Domain.Exceptions;
using ApiGrpc.Domain.Repositories;
using ApiGrpc.Domain.Repositories.Base;
using AutoMapper;
using MediatR;

namespace ApiGrpc.Application.Commands.Establishment
{
    public record UpdateEstabelecimentoCommand(
         Guid Id,
         string RazaoSocial,
         string NomeFantasia,
         string CNPJ,
         string Telefone,
         string Email,
         string? UrlImagem,
         string Descricao,
         string? InscricaoEstadual,
         string? InscricaoMunicipal,
         string? Website,
         string? RedeSocial,
         Guid CategoriaId
     ) : IRequest<EstabelecimentoDto>;

    public class UpdateEstabelecimentoCommandHandler : IRequestHandler<UpdateEstabelecimentoCommand, EstabelecimentoDto>
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateEstabelecimentoCommandHandler(
            IEstabelecimentoRepository estabelecimentoRepo,
            ICategoriaRepository categoriaRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _estabelecimentoRepo = estabelecimentoRepo;
            _categoriaRepo = categoriaRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EstabelecimentoDto> Handle(UpdateEstabelecimentoCommand request, CancellationToken cancellationToken)
        {
            // Buscar estabelecimento existente
            var estabelecimento = await _estabelecimentoRepo.GetByIdAsync(request.Id);
            if (estabelecimento == null)
                throw new NotFoundException("Estabelecimento não encontrado");

            // Validar CNPJ único (se alterado)
            if (estabelecimento.CNPJ != request.CNPJ && await _estabelecimentoRepo.CNPJExisteAsync(request.CNPJ))
                throw new ConflictException("CNPJ já está em uso");

            // Validar E-mail único (se alterado)
            if (estabelecimento.Email != request.Email && await _estabelecimentoRepo.EmailExisteAsync(request.Email))
                throw new ConflictException("E-mail já está em uso");

            // Validar categoria
            var categoria = await _categoriaRepo.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada");

            // Atualizar campos
            _mapper.Map(request, estabelecimento);

            // Persistir mudanças
            await _estabelecimentoRepo.UpdateAsync(estabelecimento);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EstabelecimentoDto>(estabelecimento);
        }
    }
}