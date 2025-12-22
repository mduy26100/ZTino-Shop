using Application.Features.Products.DTOs.ProductImages;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
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
