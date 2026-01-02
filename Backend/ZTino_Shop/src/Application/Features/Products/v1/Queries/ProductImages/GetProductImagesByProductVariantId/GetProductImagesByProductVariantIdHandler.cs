using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductVariantId
{
    public class GetProductImagesByProductVariantIdHandler : IRequestHandler<GetProductImagesByProductVariantIdQuery, IEnumerable<ProductImageDto>>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly IMapper _mapper;

        public GetProductImagesByProductVariantIdHandler(
            IProductImageRepository productImageRepository,
            IProductVariantRepository productVariantRepository,
            IMapper mapper)
        {
            _productImageRepository = productImageRepository;
            _productVariantRepository = productVariantRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductImageDto>> Handle(GetProductImagesByProductVariantIdQuery request, CancellationToken cancellationToken)
        {
            var variant = await _productVariantRepository.GetByIdAsync(request.variantId, cancellationToken);

            if (variant == null)
                throw new NotFoundException($"Product Variant Id {request.variantId} not found.");

            var productImages = await _productImageRepository.FindAsync(
                pi => pi.ProductColorId == variant.ProductColorId,
                true,
                cancellationToken);

            if (!productImages.Any())
                return Enumerable.Empty<ProductImageDto>();

            return _mapper.Map<IEnumerable<ProductImageDto>>(productImages.OrderBy(i => i.DisplayOrder));
        }
    }
}