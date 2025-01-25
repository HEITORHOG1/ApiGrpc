namespace ApiGrpc.Application.DTOs.Category
{
    public record CreateCategoriaDto(
     string Nome,
     string Descricao);

    public record CategoriaDto(
        Guid Id,
        string Nome,
        string Descricao
    );
}