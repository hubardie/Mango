using AutoMapper;
using Mango.Services.OrderAPI.Models;
using Mango.Services.OrderAPI.Models.Dto;

namespace Mango.Services.OrderAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<OrderHeaderDto, CartHeaderDto>()
                .ForMember(dest => dest.CartTotal, ori => ori.MapFrom(src=> src.OrderTotal)).ReverseMap();
                config.CreateMap<CartDetailDto, OrderDetailDto>()
                .ForMember(dest => dest.ProductName, ori => ori.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.Price, ori => ori.MapFrom(src => src.Product.Price));
                config.CreateMap<OrderDetailDto, CartDetailDto>();


                config.CreateMap<OrderHeader, OrderHeaderDto>().ReverseMap();
                config.CreateMap<OrderDetail, OrderDetailDto>().ReverseMap();
                // config.CreateMap<Product, ProductDto>(); reversemap hace éste también
            });
            return mappingConfig;

         }
    }
}
