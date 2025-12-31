using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;

namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductVariantId;

public class GetProductImagesByProductVariantIdHandler : IRequestHandler<GetProductImagesByProductVariantIdQuery, IEnumerable<ProductImageDto>>
{
    private readonly IProductImageRepository _productImageRepository;
    private readonly IMapper _mapper;

    public GetProductImagesByProductVariantIdHandler(IProductImageRepository productImageRepository,
        IMapper mapper)
    {
        _productImageRepository = productImageRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductImageDto>> Handle(GetProductImagesByProductVariantIdQuery request, CancellationToken cancellationToken)
    {
        var productImages = await _productImageRepository.FindAsync(
            pi => pi.ProductVariantId == request.variantId,
            cancellationToken: cancellationToken);

        if (!productImages.Any())
            throw new NotFoundException(
                $"No product images found for Product Variant Id {request.variantId}.");

        return _mapper.Map<IEnumerable<ProductImageDto>>(productImages);
    }
}