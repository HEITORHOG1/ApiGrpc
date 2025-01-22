namespace ApiGrpc.Application.DTOs.Login
{
    public record AuthResponseDto(
     string Token,
     string RefreshToken,
     string Email,
     string FirstName,
     string LastName,
     string Role);
}