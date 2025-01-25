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
                .ForMember(dest => dest.HorarioFuncionamento, opt => opt.Ignore()) // Será criado separadamente
                .ForMember(dest => dest.Endereco, opt => opt.Ignore()); // Será criado via command específico

            CreateMap<Estabelecimento, EstabelecimentoDto>().ReverseMap();
        }
    }
}