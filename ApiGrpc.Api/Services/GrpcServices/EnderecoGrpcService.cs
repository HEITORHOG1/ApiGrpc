using ApiGrpc.Application.Commands.Address;
using ApiGrpc.Application.DTOs.Address;
using ApiGrpc.Application.Queries.Address;
using ApiGrpc.Domain.Exceptions;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class EnderecoGrpcService : EnderecoGrpc.EnderecoGrpcBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EnderecoGrpcService> _logger;

        public EnderecoGrpcService(IMediator mediator, ILogger<EnderecoGrpcService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<EnderecoResponse> CreateEndereco(CreateEnderecoRequest request, ServerCallContext context)
        {
            try
            {
                if (!Guid.TryParse(request.UsuarioId, out var usuarioId))
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "UsuarioId inválido"));

                Guid? estabelecimentoId = null;
                if (!string.IsNullOrEmpty(request.EstabelecimentoId))
                {
                    if (!Guid.TryParse(request.EstabelecimentoId, out var parsedEstabelecimentoId))
                        throw new RpcException(new Status(StatusCode.InvalidArgument, "EstabelecimentoId inválido"));
                    estabelecimentoId = parsedEstabelecimentoId;
                }

                var command = new AddEnderecoCommand(
                    request.Logradouro,
                    request.Numero,
                    request.Complemento,
                    request.Bairro,
                    request.Cidade,
                    request.Estado,
                    request.Cep,
                    request.IsEstabelecimento,
                    usuarioId,
                    estabelecimentoId,
                    request.Latitude,
                    request.Longitude,
                    request.RaioEntregaKm);

                var result = await _mediator.Send(command);
                return MapToResponse(result);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Erro de domínio ao criar endereço");
                throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro interno ao criar endereço");
                throw new RpcException(new Status(StatusCode.Internal, "Erro interno ao processar a requisição"));
            }
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