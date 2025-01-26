namespace ApiGrpc.Application.DTOs.Address
{
    public record CreateEnderecoDto(
                string Logradouro,
                string Numero,
                string Complemento,
                string Bairro,
                string Cidade,
                string Estado,
                string Cep,
                bool IsEstabelecimento,
                Guid? UsuarioId,
                Guid? EstabelecimentoId,
                double Latitude,
                double Longitude,
                double? RaioEntregaKm
            );
}