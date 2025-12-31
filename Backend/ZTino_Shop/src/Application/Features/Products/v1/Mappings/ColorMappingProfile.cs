using Application.Features.Products.v1.DTOs.Colors;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Mappings
{
    public class ColorMappingProfile : Profile
    {
        public ColorMappingProfile()
        {
            CreateMap<Color, UpsertColorDto>().ReverseMap();
            CreateMap<Color, ColorDto>().ReverseMap();
        }
    }
}
