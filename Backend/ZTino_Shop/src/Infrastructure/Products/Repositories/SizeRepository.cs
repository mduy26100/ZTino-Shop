using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Persistence;

namespace Infrastructure.Products.Repositories
{
    public class SizeRepository : Repository<Size, int>, ISizeRepository
    {
        public SizeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}