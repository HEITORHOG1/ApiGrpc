using ApiGrpc.Application.DTOs.Category;
using ApiGrpc.Domain.Entities;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            // Mapeamento de CreateCategoriaDto para Categoria
            CreateMap<CreateCategoriaDto, Categoria>()
                .ConstructUsing(dto => new Categoria(dto.Nome, dto.Descricao)).ReverseMap();

            // Mapeamento de CreateCategoriaDto para Categoria
            CreateMap<CategoriaDto, Categoria>()
                .ConstructUsing(dto => new Categoria( dto.Nome, dto.Descricao)).ReverseMap();
        }
    }
}