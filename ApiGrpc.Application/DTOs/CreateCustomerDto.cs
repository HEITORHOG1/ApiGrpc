﻿namespace ApiGrpc.Application.DTOs
{
    public record CreateCustomerDto(
     string Name,
     string Email,
     string Phone
 );
}