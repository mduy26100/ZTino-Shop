using Application.Features.Products.DTOs.Colors;

namespace Application.Features.Products.Commands.Colors.UpdateColor
{
    public record UpdateColorCommand(UpsertColorDto Dto) : IRequest<UpsertColorDto>;
}
