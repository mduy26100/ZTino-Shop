using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Services;

namespace Application.Features.Products.Queries.Products.GetProductDetail
{
    public class GetProductDetailHandler : IRequestHandler<GetProductDetailQuery, ProductDetailDto?>
    {
        private readonly IProductQueryService _productQueryService;

        public GetProductDetailHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<ProductDetailDto?> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            return await _productQueryService.GetProductDetailAsync(request.id, cancellationToken);
        }
    }
}
