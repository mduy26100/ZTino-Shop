using Application.Features.Products.DTOs.Colors;
using Domain.Models.Products;

namespace Application.Features.Products.Mappings
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
