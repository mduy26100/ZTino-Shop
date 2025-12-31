using Application.Common.Interfaces.Persistence.Base;
using Application.Features.Products.v1.DTOs.Products;
using Domain.Models.Products;

namespace Application.Features.Products.v1.Repositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
