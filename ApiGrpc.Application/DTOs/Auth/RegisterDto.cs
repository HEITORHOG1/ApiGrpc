﻿namespace ApiGrpc.Application.DTOs.Auth
{
    public record RegisterDto(
     string Email,
     string Password,
     string FirstName,
     string LastName,
     string Role = "Cliente");
}