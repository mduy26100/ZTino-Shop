using Application.Features.Products.v1.DTOs.ProductColor;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class ProductColorMappingProfile : Profile
    {
        public ProductColorMappingProfile()
        {
            CreateMap<ProductColor, ProductColorDto>().ReverseMap();
        }
    }
}
