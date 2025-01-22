namespace ApiGrpc.Application.DTOs.Customer
{
    public record UpdateCustomerDto(
    Guid Id,
    string Name,
    string Email,
    string Phone
);
}