using Application.Features.Products.v1.DTOs.ProductVariants;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class ProductVariantMappingProfile : Profile
    {
        public ProductVariantMappingProfile()
        {
            CreateMap<UpsertProductVariantDto, ProductVariant>().ReverseMap();
            CreateMap<ProductVariant, ProductVariantDto>().ReverseMap();
        }
    }
}
