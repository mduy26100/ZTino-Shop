using Application.Features.Products.DTOs.Sizes;

namespace Application.Features.Products.Commands.Sizes.CreateSize
{
    public record CreateSizeCommand(UpsertSizeDto Dto) : IRequest<UpsertSizeDto>;
}
