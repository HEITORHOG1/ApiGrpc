namespace ApiGrpc.Application.DTOs.Auth
{
    public record UserDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt);
}