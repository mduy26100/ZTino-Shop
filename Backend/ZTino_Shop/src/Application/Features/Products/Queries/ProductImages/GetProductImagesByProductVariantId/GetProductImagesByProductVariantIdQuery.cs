using Application.Features.Products.DTOs.ProductImages;

namespace Application.Features.Products.Queries.ProductImages.GetProductImagesByProductVariantId;

public record GetProductImagesByProductVariantIdQuery(int variantId) : IRequest<IEnumerable<ProductImageDto>>;