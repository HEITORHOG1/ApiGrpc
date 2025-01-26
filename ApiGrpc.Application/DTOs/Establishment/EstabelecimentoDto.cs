using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Application.DTOs.Category;

namespace ApiGrpc.Application.DTOs.Establishment
{
    public record EstabelecimentoDto(
        Guid Id,
        Guid UsuarioId,
        string RazaoSocial,
        string NomeFantasia,
        string CNPJ,
        string Telefone,
        string Email,
        bool Status,
        string? UrlImagem,
        string Descricao,
        string? InscricaoEstadual,
        string? InscricaoMunicipal,
        string? Website,
        string? RedeSocial,
        Guid? CategoriaId,
        CategoriaDto Categoria,
        EnderecoDto Endereco
    );
}