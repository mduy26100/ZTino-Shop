using Application.Features.Products.v1.DTOs.ProductImages;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class ProductImageMappingProfile : Profile
    {
        public ProductImageMappingProfile()
        {
            CreateMap<ProductImage, UpsertProductImageDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
        }
    }
}
