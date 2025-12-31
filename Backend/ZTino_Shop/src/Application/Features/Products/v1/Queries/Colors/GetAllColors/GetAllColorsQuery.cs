using Application.Features.Products.v1.DTOs.Colors;

namespace Application.Features.Products.v1.Queries.Colors.GetAllColors
{
    public record GetAllColorsQuery : IRequest<IEnumerable<ColorDto>>;
}
