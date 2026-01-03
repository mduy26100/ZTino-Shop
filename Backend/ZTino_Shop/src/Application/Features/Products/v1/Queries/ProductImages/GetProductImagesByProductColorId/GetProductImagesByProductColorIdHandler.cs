using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductColorId
{
    public class GetProductImagesByProductColorIdHandler : IRequestHandler<GetProductImagesByProductColorIdQuery, IEnumerable<ProductImageDto>>
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly IMapper _mapper;

        public GetProductImagesByProductColorIdHandler(
            IProductImageRepository productImageRepository,
            IMapper mapper)
        {
            _productImageRepository = productImageRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductImageDto>> Handle(GetProductImagesByProductColorIdQuery request, CancellationToken cancellationToken)
        {
            var productImages = await _productImageRepository.FindAsync(
                pi => pi.ProductColorId == request.productColorId,
                true,
                cancellationToken);

            if (!productImages.Any())
                return Enumerable.Empty<ProductImageDto>();

            return _mapper.Map<IEnumerable<ProductImageDto>>(productImages.OrderBy(i => i.DisplayOrder));
        }
    }
}