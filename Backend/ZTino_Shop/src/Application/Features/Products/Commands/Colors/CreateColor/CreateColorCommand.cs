using Application.Features.Products.DTOs.Colors;

namespace Application.Features.Products.Commands.Colors.CreateColor
{
    public record CreateColorCommand(UpsertColorDto Dto) : IRequest<UpsertColorDto>;
}
