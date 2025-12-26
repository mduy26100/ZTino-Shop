using Application.Common.Interfaces.Persistence.Base;
using Application.Features.Products.DTOs.Products;
using Domain.Models.Products;

namespace Application.Features.Products.Repositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
