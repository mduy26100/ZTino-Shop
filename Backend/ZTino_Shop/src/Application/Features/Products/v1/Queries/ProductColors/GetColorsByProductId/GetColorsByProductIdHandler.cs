using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.ProductColors.GetColorsByProductId
{
    public class GetColorsByProductIdHandler : IRequestHandler<GetColorsByProductIdQuery, List<ProductColorSummaryDto>>
    {
        private readonly IProductColorRepository _productColorRepository;

        public GetColorsByProductIdHandler(IProductColorRepository productColorRepository)
        {
            _productColorRepository = productColorRepository;
        }

        public async Task<List<ProductColorSummaryDto>> Handle(GetColorsByProductIdQuery request, CancellationToken cancellationToken)
        {
            var query = _productColorRepository.GetAll(pc => pc.ProductId == request.productId);

            var result = query.Select(pc => new ProductColorSummaryDto
            {
                Id = pc.Id,
                Color = new ColorDto
                {
                    Id = pc.Color.Id,
                    Name = pc.Color.Name
                }
            }).ToList();

            return await Task.FromResult(result);
        }
    }
}
