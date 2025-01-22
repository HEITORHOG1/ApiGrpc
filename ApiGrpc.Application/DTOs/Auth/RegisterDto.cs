namespace ApiGrpc.Application.DTOs.Login
{
    public record RegisterDto(
     string Email,
     string Password,
     string FirstName,
     string LastName);
}