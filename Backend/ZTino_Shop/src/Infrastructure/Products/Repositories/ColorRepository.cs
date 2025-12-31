using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using Infrastructure.Common.Interfaces.Persistence.Base;
using Infrastructure.Data;

namespace Infrastructure.Products.Repositories
{
    public class ColorRepository : Repository<Color, int>, IColorRepository
    {
        public ColorRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
