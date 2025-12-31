using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductVariantId;

public record GetProductImagesByProductVariantIdQuery(int variantId) : IRequest<IEnumerable<ProductImageDto>>;