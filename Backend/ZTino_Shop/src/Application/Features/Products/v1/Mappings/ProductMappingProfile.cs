using Application.Features.Products.v1.DTOs.Products;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, UpsertProductDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MainImageUrl, opt => opt.Ignore());

            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
