using ApiGrpc.Application.Commands.Establishment;
using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Application.Queries.Establishment;
using Grpc.Core;
using MediatR;

namespace ApiGrpc.Api.Services.GrpcServices
{
    public class EstabelecimentoGrpcService : EstabelecimentoGrpc.EstabelecimentoGrpcBase
    {
        private readonly IMediator _mediator;

        public EstabelecimentoGrpcService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<EstabelecimentoResponse> CreateEstabelecimento(
            CreateEstabelecimentoRequest request, ServerCallContext context)
        {
            var command = new AddEstabelecimentoCommand(
                Guid.Parse(request.UsuarioId),
                request.RazaoSocial,
                request.NomeFantasia,
                request.Cnpj,
                request.Telefone,
                request.Email,
                request.UrlImagem,
                request.Descricao,
                request.InscricaoEstadual,
                request.InscricaoMunicipal,
                request.Website,
                request.RedeSocial,
                Guid.Parse(request.CategoriaId)
            );

            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<EstabelecimentoResponse> UpdateEstabelecimento(
            UpdateEstabelecimentoRequest request, ServerCallContext context)
        {
            var command = new UpdateEstabelecimentoCommand(
                Guid.Parse(request.Id),
                request.RazaoSocial,
                request.NomeFantasia,
                request.Cnpj,
                request.Telefone,
                request.Email,
                request.UrlImagem,
                request.Descricao,
                request.InscricaoEstadual,
                request.InscricaoMunicipal,
                request.Website,
                request.RedeSocial,
                Guid.Parse(request.CategoriaId)
            );

            var result = await _mediator.Send(command);
            return MapToResponse(result);
        }

        public override async Task<EstabelecimentoResponse> GetEstabelecimento(
            GetEstabelecimentoRequest request, ServerCallContext context)
        {
            var query = new GetEstabelecimentoByIdQuery(Guid.Parse(request.Id));
            var result = await _mediator.Send(query);
            return MapToResponse(result);
        }

        public override async Task<EstabelecimentoListResponse> GetEstabelecimentosByUsuario(
            GetEstabelecimentosByUsuarioRequest request, ServerCallContext context)
        {
            var query = new GetEstabelecimentosByUsuarioQuery(Guid.Parse(request.UsuarioId));
            var results = await _mediator.Send(query);

            var response = new EstabelecimentoListResponse();
            response.Estabelecimentos.AddRange(results.Select(MapToResponse));
            return response;
        }




        private static EstabelecimentoResponse MapToResponse(EstabelecimentoDto dto)
        {
            return new EstabelecimentoResponse
            {
                Id = dto.Id.ToString(),
                RazaoSocial = dto.RazaoSocial,
                NomeFantasia = dto.NomeFantasia,
                Cnpj = dto.CNPJ,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Descricao = dto.Descricao ?? "",
                UsuarioId = dto.UsuarioId.ToString(),
                CategoriaId = dto.CategoriaId.ToString(),
                UrlImagem = dto.UrlImagem ?? "",
                InscricaoEstadual = dto.InscricaoEstadual ?? "",
                InscricaoMunicipal = dto.InscricaoMunicipal ?? "",
                Website = dto.Website ?? "",
                RedeSocial = dto.RedeSocial ?? "",
                Status = dto.Status
            };
        }
    }
}
