namespace ApiGrpc.Application.DTOs.Customer
{
    public record CreateCustomerDto(
     string Name,
     string Email,
     string Phone
 );
}