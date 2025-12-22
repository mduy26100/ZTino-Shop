using Application.Features.Products.DTOs.Sizes;

namespace Application.Features.Products.Commands.Sizes.UpdateSize
{
    public record UpdateSizeCommand(UpsertSizeDto Dto) : IRequest<UpsertSizeDto>;
}
