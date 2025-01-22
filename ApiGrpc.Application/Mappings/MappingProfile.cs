using ApiGrpc.Application.DTOs.Auth;
using ApiGrpc.Application.DTOs.Customer;
using ApiGrpc.Domain.Entities;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<UpdateCustomerDto, Customer>();

            // Mapeamento para User
            CreateMap<ApplicationUser, UserDto>()
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Email", opt => opt.MapFrom(src => src.Email))
                .ForCtorParam("FirstName", opt => opt.MapFrom(src => src.FirstName))
                .ForCtorParam("LastName", opt => opt.MapFrom(src => src.LastName))
                .ForCtorParam("CreatedAt", opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}