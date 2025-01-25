namespace ApiGrpc.Application.DTOs.Establishment
{
    public record CreateEstabelecimentoDto(
    Guid UsuarioId,
    string RazaoSocial,
    string NomeFantasia,
    string CNPJ,  // Deve ser obrigatório
    string Telefone,
    string Email,
    string Descricao,
    Guid CategoriaId,  // Deve ser obrigatório
    string? UrlImagem = null,
    string? InscricaoEstadual = null,
    string? InscricaoMunicipal = null,
    string? Website = null,
    string? RedeSocial = null
);
}