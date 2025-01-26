namespace ApiGrpc.Application.DTOs.Auth
{
    public record AuthResponseDto(
            string AccessToken,
            string RefreshToken,
            string Email,
            string FirstName,
            string LastName,
            string Role,
            string UserId);
}