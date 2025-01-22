namespace ApiGrpc.Application.DTOs.Customer
{
    public record CustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    bool Active,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
}