using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Services
{
    public interface ICartQueryService
    {
        Task<CartDto?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<CartDto?> GetCartByIdAsync(Guid cartId, CancellationToken cancellationToken);
    }
}
