using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Expressions;
using Application.Features.Products.Services;
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
    }
}
