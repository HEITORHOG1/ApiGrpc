namespace ApiGrpc.Application.DTOs
{
    public record UpdateCustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Phone
);
}