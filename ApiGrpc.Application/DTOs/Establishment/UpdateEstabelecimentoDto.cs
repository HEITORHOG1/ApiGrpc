namespace ApiGrpc.Application.DTOs.Establishment
{
    public record UpdateEstabelecimentoDto(
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
     );
}