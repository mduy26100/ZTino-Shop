using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Expressions;
using Ardalis.Specification;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Specifications
{
    public class ProductDetailSpec : Specification<Product, ProductDetailDto>
    {
        public ProductDetailSpec(int id)
        {
            Query.Where(p => p.Id == id);
            Query.AsNoTracking();
            Query.AsSplitQuery();
            Query.Select(ProductExpressions.MapToDetailDto);
        }

        public ProductDetailSpec(string slug)
        {
            Query.Where(p => p.Slug == slug);
            Query.AsNoTracking();
            Query.AsSplitQuery();
            Query.Select(ProductExpressions.MapToDetailDto);
        }
    }
}
