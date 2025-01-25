using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Domain.Entities;
using ApiGrpc.Domain.Repositories.Base;
using ApiGrpc.Domain.Repositories;
using AutoMapper;
using MediatR;
using ApiGrpc.Domain.Exceptions;

namespace ApiGrpc.Application.Commands.Establishment
{
    public record AddEstabelecimentoCommand(
        Guid UsuarioId,
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

    public class AddEstabelecimentoCommandHandler : IRequestHandler<AddEstabelecimentoCommand, EstabelecimentoDto>
    {
        private readonly IEstabelecimentoRepository _estabelecimentoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddEstabelecimentoCommandHandler(
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

        public async Task<EstabelecimentoDto> Handle(AddEstabelecimentoCommand request, CancellationToken cancellationToken)
        {
            // Validação 1: CNPJ único
            if (await _estabelecimentoRepo.CNPJExisteAsync(request.CNPJ))
                throw new ConflictException("CNPJ já cadastrado");

            // Validação 2: Categoria existe
            var categoria = await _categoriaRepo.GetByIdAsync(request.CategoriaId);
            if (categoria == null)
                throw new NotFoundException("Categoria não encontrada");

            // Validação 3: E-mail único (se aplicável)
            if (await _estabelecimentoRepo.EmailExisteAsync(request.Email))
                throw new ConflictException("E-mail já cadastrado");

            // Mapeamento
            var estabelecimento = _mapper.Map<Estabelecimento>(request);

            // Persistência
            await _estabelecimentoRepo.AddAsync(estabelecimento);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<EstabelecimentoDto>(estabelecimento);
        }
    }
}