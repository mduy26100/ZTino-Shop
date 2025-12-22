using Application.Features.Products.DTOs.ProductVariants;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
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
