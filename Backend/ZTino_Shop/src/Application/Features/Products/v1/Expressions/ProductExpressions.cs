using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.DTOs.Sizes;
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

                ProductColors = p.ProductColors.Select(pc => new ProductColorDto
                {
                    Id = pc.Id,
                    Color = new ColorDto
                    {
                        Id = pc.Color.Id,
                        Name = pc.Color.Name
                    },
                    Images = pc.Images
                        .OrderBy(i => i.DisplayOrder)
                        .Select(i => new ProductImageDto
                        {
                            Id = i.Id,
                            ImageUrl = i.ImageUrl,
                            IsMain = i.IsMain,
                            DisplayOrder = i.DisplayOrder
                        }).ToList(),
                    Variants = pc.ProductVariants.Select(pv => new ProductVariantDto
                    {
                        Id = pv.Id,
                        Price = pv.Price,
                        StockQuantity = pv.StockQuantity,
                        IsActive = pv.IsActive,
                        Size = new SizeDto
                        {
                            Id = pv.Size.Id,
                            Name = pv.Size.Name
                        }
                    }).ToList()
                }).ToList()
            };
    }
}
