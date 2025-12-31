using Application.Features.Products.v1.DTOs.Colors;

namespace Application.Features.Products.v1.Commands.Colors.UpdateColor
{
    public record UpdateColorCommand(UpsertColorDto Dto) : IRequest<UpsertColorDto>;
}
