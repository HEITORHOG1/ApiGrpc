namespace ApiGrpc.Application.DTOs.Auth
{
    public record UpdateUserDto(
        string Email,
        string FirstName,
        string LastName,
        string Role
    );
}