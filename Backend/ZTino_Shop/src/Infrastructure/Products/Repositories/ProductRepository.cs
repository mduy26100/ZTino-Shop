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
    }
}
