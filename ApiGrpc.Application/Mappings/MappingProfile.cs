using ApiGrpc.Application.DTOs;
using ApiGrpc.Domain.Entities;
using AutoMapper;

namespace ApiGrpc.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<CreateCustomerDto, Customer>().ReverseMap();
            CreateMap<UpdateCustomerDto, Customer>().ReverseMap();
        }
    }
}