namespace ApiGrpc.Application.DTOs.Login
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