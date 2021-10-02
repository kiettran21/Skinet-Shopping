using AutoMapper;
using Core.Entities;
using API.Dtos;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Product, ProductReturnDto>()
                .ForMember(x => x.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(x => x.ProductType, o => o.MapFrom(p => p.ProductType.Name))
                .ForMember(x => x.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

            // Revert Map help can reverse mapper
            CreateMap<Address, AddressDto>().ReverseMap();

            // Create Mapper with order aggerate
            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            // Create Mapper with rating
            CreateMap<Rating, RatingReturnDto>();
        }
    }
}