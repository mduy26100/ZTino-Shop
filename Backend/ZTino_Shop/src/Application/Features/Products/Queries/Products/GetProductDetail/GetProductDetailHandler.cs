using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Queries.Products.GetProductDetail
{
    public class GetProductDetailHandler : IRequestHandler<GetProductDetailQuery, ProductDetailDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDetailDto?> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _productRepository.GetProductDetailAsync(request.id, cancellationToken);
            return entity;
        }
    }
}
