using Application.Features.Products.v1.DTOs.Sizes;

namespace Application.Features.Products.v1.Commands.Sizes.UpdateSize
{
    public record UpdateSizeCommand(UpsertSizeDto Dto) : IRequest<UpsertSizeDto>;
}
