using Application.Features.Products.v1.DTOs.Products;
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
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(ProductExpressions.MapToDetailDto);

            var dto = await query
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            return dto;
        }

        public async Task<ProductDetailDto?> GetProductDetailBySlugAsync(string slug, CancellationToken cancellationToken)
        {
            var query = _context.Products
                .AsNoTracking()
                .Where(p => p.Slug == slug)
                .Select(ProductExpressions.MapToDetailDto);

            var dto = await query
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            return dto;
        }
    }
}
