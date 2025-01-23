using ApiGrpc.Application.Commands.Auth;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class AuthGrpcService : AuthGrpc.AuthGrpcBase
    {
        private readonly IMediator _mediator;

        public AuthGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<AuthResponse> Login(LoginRequest request, ServerCallContext context)
        {
            var command = new LoginCommand(request.Email, request.Password);
            var result = await _mediator.Send(command);

            return new AuthResponse
            {
                Token = result.AccessToken,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Role = result.Role
            };
        }

        public override async Task<AuthResponse> Register(RegisterRequest request, ServerCallContext context)
        {
            var command = new RegisterCommand(
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.Role);

            var result = await _mediator.Send(command);

            return new AuthResponse
            {
                Token = result.AccessToken,
                Email = result.Email,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Role = result.Role
            };
        }
    }
}