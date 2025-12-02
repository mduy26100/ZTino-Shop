using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Repositories;

namespace Application.Features.Products.Queries.Products.GetAllProducts
{
    public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var listProducts = await _productRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ProductDto>>(listProducts);
        }
    }
}
