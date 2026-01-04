using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Expressions;
using Application.Features.Carts.v1.Services;
using Infrastructure.Data;

namespace Infrastructure.Carts.Services
{
    public class CartQueryService : ICartQueryService
    {
        private readonly ApplicationDbContext _context;

        public CartQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .AsNoTracking()
                .Where(c => c.UserId == userId && c.IsActive)
                .Select(c => new
                {
                    c.Id,
                    Items = c.CartItems
                        .AsQueryable()
                        .Select(CartExpressions.MapToCartItemDto)
                        .ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is null)
            {
                return null;
            }

            return BuildCartDto(cart.Id, cart.Items);
        }

        public async Task<CartDto?> GetCartByIdAsync(Guid cartId, CancellationToken cancellationToken)
        {
            var cart = await _context.Carts
                .AsNoTracking()
                .Where(c => c.Id == cartId && c.IsActive)
                .Select(c => new
                {
                    c.Id,
                    Items = c.CartItems
                        .AsQueryable()
                        .Select(CartExpressions.MapToCartItemDto)
                        .ToList()
                })
                .AsSplitQuery()
                .FirstOrDefaultAsync(cancellationToken);

            if (cart is null)
            {
                return null;
            }

            return BuildCartDto(cart.Id, cart.Items);
        }

        private static CartDto BuildCartDto(Guid cartId, List<CartItemDto> items)
        {
            return new CartDto
            {
                CartId = cartId,
                Items = items,
                TotalPrice = items.Sum(i => i.ItemTotal),
                TotalItems = items.Sum(i => i.Quantity)
            };
        }
    }
}
