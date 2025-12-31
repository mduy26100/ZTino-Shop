using Application.Features.Products.v1.DTOs.Colors;

namespace Application.Features.Products.v1.Commands.Colors.CreateColor
{
    public record CreateColorCommand(UpsertColorDto Dto) : IRequest<UpsertColorDto>;
}
