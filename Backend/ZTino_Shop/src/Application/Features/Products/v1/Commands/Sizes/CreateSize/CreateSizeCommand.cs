using Application.Features.Products.v1.DTOs.Sizes;

namespace Application.Features.Products.v1.Commands.Sizes.CreateSize
{
    public record CreateSizeCommand(UpsertSizeDto Dto) : IRequest<UpsertSizeDto>;
}
