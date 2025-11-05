using Application.Features.Products.DTOs.Products;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, UpsertProductDto>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MainImageUrl, opt => opt.Ignore());
        }
    }
}
