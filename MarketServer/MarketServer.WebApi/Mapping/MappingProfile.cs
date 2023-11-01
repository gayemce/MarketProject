using AutoMapper;
using MarketServer.WebApi.Dtos;
using MarketServer.WebApi.Models;

namespace MarketServer.WebApi.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>();
        CreateMap<Product, ProductDto>();
    }
}
