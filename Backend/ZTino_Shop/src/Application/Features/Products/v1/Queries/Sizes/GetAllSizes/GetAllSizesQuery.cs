using Application.Features.Products.v1.DTOs.Sizes;

namespace Application.Features.Products.v1.Queries.Sizes.GetAllSizes
{
    public record GetAllSizesQuery() : IRequest<IEnumerable<SizeDto>>;
}
