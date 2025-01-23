using ApiGrpc.Application.DTOs.Address;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class EnderecoProfile : Profile
    {
        public EnderecoProfile()
        {
            CreateMap<Endereco, EnderecoDto>();

            CreateMap<CreateEnderecoDto, Endereco>()
                .ConstructUsing(dto => new Endereco(
                    dto.Logradouro,
                    dto.Numero,
                    dto.Complemento,
                    dto.Bairro,
                    dto.Cidade,
                    dto.Estado,
                    dto.Cep,
                    dto.IsEstabelecimento,
                    dto.UsuarioId,
                    dto.Latitude,
                    dto.Longitude,
                    dto.RaioEntregaKm));
        }
    }
}