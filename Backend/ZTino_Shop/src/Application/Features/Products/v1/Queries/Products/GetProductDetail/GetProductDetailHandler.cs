using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Repositories;
using Application.Features.Products.v1.Specifications;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetail
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
            var spec = new ProductDetailSpec(request.id);

            var productDto = await _productRepository.FirstOrDefaultAsync(spec, cancellationToken);

            return productDto;
        }
    }
}
