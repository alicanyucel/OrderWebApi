using AutoMapper;
using OrderWebApi.Models.Dtos;
using OrderWebApi.Models.Entities;

namespace OrderWebApi.Models.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Order, OrderDetail>().ReverseMap();
            CreateMap<ProductDetailDto, OrderDetail>().ReverseMap();
        }
    }
}