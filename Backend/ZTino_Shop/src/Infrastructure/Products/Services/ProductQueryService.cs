using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Expressions;
using Application.Features.Products.v1.Services;
using Infrastructure.Data;

namespace Infrastructure.Products.Services
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly ApplicationDbContext _context;

        public ProductQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(ProductExpressions.MapToDetailDto)
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ProductDetailDto?> GetProductDetailBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var productEntity = await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Variants).ThenInclude(v => v.Color)
                .Include(p => p.Variants).ThenInclude(v => v.Size)
                .Include(p => p.Variants).ThenInclude(v => v.Images)
                .Where(p => p.Slug == slug)
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            if (productEntity == null) return null;

            var dto = new ProductDetailDto
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                Slug = productEntity.Slug,
                BasePrice = productEntity.BasePrice,
                Description = productEntity.Description,
                MainImageUrl = productEntity.MainImageUrl,
                IsActive = productEntity.IsActive,
                CreatedAt = productEntity.CreatedAt,
                UpdatedAt = productEntity.UpdatedAt,

                Category = new CategoryDto
                {
                    Id = productEntity.Category.Id,
                    Name = productEntity.Category.Name,
                    Slug = productEntity.Category.Slug
                },

                Variants = new List<ProductVariantDto>(),

                VariantGroups = productEntity.Variants
                    .Where(v => v.IsActive)
                    .GroupBy(v => v.Color.Id)
                    .Select(g =>
                    {
                        var firstVariant = g.First();
                        return new ProductVariantGroupDto
                        {
                            ColorId = firstVariant.Color.Id,
                            ColorName = firstVariant.Color.Name,

                            Images = g.SelectMany(v => v.Images)
                                      .DistinctBy(i => i.Id)
                                      .OrderBy(i => i.DisplayOrder)
                                      .Select(i => new ProductImageDto
                                      {
                                          Id = i.Id,
                                          ImageUrl = i.ImageUrl,
                                          IsMain = i.IsMain,
                                          DisplayOrder = i.DisplayOrder
                                      }).ToList(),

                            Options = g.Select(v => new ProductVariantOptionDto
                            {
                                VariantId = v.Id,
                                SizeId = v.Size.Id,
                                SizeName = v.Size.Name,
                                Price = v.Price,
                                StockQuantity = v.StockQuantity,
                                IsActive = v.IsActive
                            })
                                      .OrderBy(o => o.SizeName)
                                      .ToList()
                        };
                    })
                    .OrderBy(g => g.ColorId)
                    .ToList()
            };

            return dto;
        }
    }
}
