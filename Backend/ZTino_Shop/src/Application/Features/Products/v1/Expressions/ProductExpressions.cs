using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.DTOs.Sizes;
using Application.Features.Products.v1.DTOs.Products;
using Domain.Models.Products;
using System.Linq.Expressions;

namespace Application.Features.Products.v1.Expressions
{
    public static class ProductExpressions
    {
        public static Expression<Func<Product, ProductDetailDto>> MapToDetailDto =>
            p => new ProductDetailDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                BasePrice = p.BasePrice,
                Description = p.Description,
                MainImageUrl = p.MainImageUrl,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,

                Category = new CategoryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name,
                    Slug = p.Category.Slug
                },

                Variants = p.Variants
                    .Select(v => new ProductVariantDto
                    {
                        Id = v.Id,
                        Price = v.Price,
                        StockQuantity = v.StockQuantity,
                        IsActive = v.IsActive,

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
                            }).ToList()
                    }).ToList()
            };
    }
}
