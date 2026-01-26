using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Repositories;
using Application.Features.Products.v1.Specifications;

namespace Application.Features.Products.v1.Queries.Products.GetProductDetailBySlug
{
    public class GetProductDetailBySlugHandler : IRequestHandler<GetProductDetailBySlugQuery, ProductDetailDto?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailBySlugHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDetailDto?> Handle(GetProductDetailBySlugQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProductDetailSpec(request.slug);

            var productDto = await _productRepository.FirstOrDefaultAsync(spec, cancellationToken);

            return productDto;
        }
    }
}
