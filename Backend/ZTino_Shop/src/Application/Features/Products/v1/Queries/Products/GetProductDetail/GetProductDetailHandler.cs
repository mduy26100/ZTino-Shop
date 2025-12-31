using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Services;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetail
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
            var product = await _productQueryService.GetProductDetailAsync(request.id, cancellationToken);

            if(product == null)
            {
                throw new NotFoundException($"Product with Id {request.id} not found.");
            }

            return product;
        }
    }
}
