using Application.Features.Products.DTOs.Colors;

namespace Application.Features.Products.Queries.Colors.GetAllColors
{
    public record GetAllColorsQuery : IRequest<IEnumerable<ColorDto>>;
}
