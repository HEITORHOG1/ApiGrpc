namespace ApiGrpc.Application.DTOs.Establishment
{
    public record CreateEstabelecimentoDto(
    string RazaoSocial,
    string NomeFantasia,
    string CNPJ, 
    string Telefone,
    string Email,
    string Descricao,
    Guid CategoriaId,
    string? UrlImagem = null,
    string? InscricaoEstadual = null,
    string? InscricaoMunicipal = null,
    string? Website = null,
    string? RedeSocial = null
);
}