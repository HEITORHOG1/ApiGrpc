using ApiGrpc.Application.Commands.Address;
using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Application.Queries.Address;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class EnderecoGrpcService : EnderecoGrpc.EnderecoGrpcBase
    {
        private readonly IMediator _mediator;

        public EnderecoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<EnderecoResponse> CreateEndereco(CreateEnderecoRequest request, ServerCallContext context)
        {
            var command = new AddEnderecoCommand(
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.IsEstabelecimento,
                Guid.Parse(request.UsuarioId),
                request.Latitude,
                request.Longitude,
                request.RaioEntregaKm);

            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<EnderecoResponse> UpdateEndereco(UpdateEnderecoRequest request, ServerCallContext context)
        {
            var command = new UpdateEnderecoCommand(
                Guid.Parse(request.Id),
                request.Logradouro,
                request.Numero,
                request.Complemento,
                request.Bairro,
                request.Cidade,
                request.Estado,
                request.Cep,
                request.Latitude,
                request.Longitude);

            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<EnderecoResponse> GetEndereco(GetEnderecoRequest request, ServerCallContext context)
        {
            var query = new GetEnderecoQuery(Guid.Parse(request.Id));
            var result = await _mediator.Send(query);
            return MapToResponse(result);
        }

        public override async Task<EnderecoListResponse> GetEnderecosByUsuario(GetEnderecosByUsuarioRequest request, ServerCallContext context)
        {
            var query = new GetEnderecosByUsuarioQuery(Guid.Parse(request.UsuarioId));
            var results = await _mediator.Send(query);

            var response = new EnderecoListResponse();
            response.Enderecos.AddRange(results.Select(MapToResponse));
            return response;
        }

        private static EnderecoResponse MapToResponse(EnderecoDto dto)
        {
            var response = new EnderecoResponse
            {
                Id = dto.Id.ToString(),
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado,
                Cep = dto.Cep,
                IsEstabelecimento = dto.IsEstabelecimento,
                UsuarioId = dto.UsuarioId.ToString(),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Status = dto.Status
            };

            if (dto.RaioEntregaKm.HasValue)
            {
                response.RaioEntregaKm = dto.RaioEntregaKm.Value;
            }

            return response;
        }
    }
}