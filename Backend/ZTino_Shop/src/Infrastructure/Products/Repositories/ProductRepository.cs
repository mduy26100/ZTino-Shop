using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.DTOs.Colors;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.DTOs.Products;
using Application.Features.Products.DTOs.ProductVariants;
using Application.Features.Products.DTOs.Sizes;
using Application.Features.Products.Repositories;
using Domain.Models.Products;
using Infrastructure.Common.Interfaces.Persistence.Base;
using Infrastructure.Data;

namespace Infrastructure.Products.Repositories
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<ProductDetailDto?> GetProductDetailAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    BasePrice = p.BasePrice,
                    Description = p.Description,
                    MainImageUrl = p.MainImageUrl,

                    Category = new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name,
                        Slug = p.Category.Slug
                    },

                    Variants = p.Variants
                        .Where(v => v.IsActive)
                        .Select(v => new ProductVariantDto
                        {
                            Id = v.Id,
                            Price = v.Price,
                            StockQuantity = v.StockQuantity,

                            Color = new ColorDto
                            {
                                Id = v.Color.Id,
                                Name = v.Color.Name
                            },

                            Size = new SizeDto
                            {
                                Id = v.Size.Id,
                                Name = v.Size.Name
                            },

                            Images = v.Images
                                .OrderBy(i => i.DisplayOrder)
                                .Select(i => new ProductImageDto
                                {
                                    Id = i.Id,
                                    ImageUrl = i.ImageUrl,
                                    IsMain = i.IsMain,
                                    DisplayOrder = i.DisplayOrder
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();
        }
    }
}
