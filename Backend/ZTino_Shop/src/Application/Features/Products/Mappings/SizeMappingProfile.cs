using Application.Features.Products.DTOs.Sizes;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
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
