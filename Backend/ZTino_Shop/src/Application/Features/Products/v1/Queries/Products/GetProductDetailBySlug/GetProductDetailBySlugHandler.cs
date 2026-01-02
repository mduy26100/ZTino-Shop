using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Services;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetailBySlug
{
    public class GetProductDetailBySlugHandler : IRequestHandler<GetProductDetailBySlugQuery, ProductDetailDto?>
    {
        private readonly IProductQueryService _productQueryService;

        public GetProductDetailBySlugHandler(IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<ProductDetailDto?> Handle(GetProductDetailBySlugQuery request, CancellationToken cancellationToken)
        {
            var product = await _productQueryService.GetProductDetailBySlugAsync(request.slug, cancellationToken);

            return product;
        }
    }
}
