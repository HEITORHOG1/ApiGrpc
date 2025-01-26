using ApiGrpc.Application.DTOs.Address;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class EnderecoProfile : Profile
    {
        public EnderecoProfile()
        {
            CreateMap<Endereco, EnderecoDto>()
                .ForMember(dest => dest.RaioEntregaKm,
                    opt => opt.MapFrom(src => src.IsEstabelecimento ? src.RaioEntregaKm : null));

            CreateMap<CreateEnderecoDto, Endereco>()
                .ConstructUsing((src, ctx) => new Endereco(
                    src.Logradouro,
                    src.Numero,
                    src.Complemento,
                    src.Bairro,
                    src.Cidade,
                    src.Estado,
                    src.Cep,
                    src.IsEstabelecimento,
                    src.UsuarioId,
                    src.EstabelecimentoId,
                    src.Latitude,
                    src.Longitude,
                    src.RaioEntregaKm));
        }
    }
}