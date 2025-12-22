using Application.Features.Products.DTOs.Sizes;

namespace Application.Features.Products.Queries.Sizes.GetAllSizes
{
    public record GetAllSizesQuery() : IRequest<IEnumerable<SizeDto>>;
}
