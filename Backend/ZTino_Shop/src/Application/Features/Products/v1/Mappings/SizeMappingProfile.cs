using Application.Features.Products.v1.DTOs.Sizes;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class SizeMappingProfile : Profile
    {
        public SizeMappingProfile()
        {
            CreateMap<Size, UpsertSizeDto>().ReverseMap();
            CreateMap<Size, SizeDto>().ReverseMap();
        }
    }
}
