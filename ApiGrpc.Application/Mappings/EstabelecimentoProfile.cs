using ApiGrpc.Application.Commands.Establishment;
using ApiGrpc.Application.DTOs.Establishment;
using ApiGrpc.Domain.Entities;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class EstabelecimentoProfile : Profile
    {
        public EstabelecimentoProfile()
        {
            CreateMap<AddEstabelecimentoCommand, Estabelecimento>()
                .ForMember(dest => dest.HorariosFuncionamento, opt => opt.Ignore()); // Será criado separadamente

            //CreateMap<Estabelecimento, EstabelecimentoDto>().ReverseMap();
            CreateMap<Estabelecimento, EstabelecimentoDto>().
                        ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco));
        }
    }
}