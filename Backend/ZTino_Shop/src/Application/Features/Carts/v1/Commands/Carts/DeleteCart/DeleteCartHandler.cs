using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Carts.v1.Repositories;
using Domain.Models.Carts;

namespace Application.Features.Carts.v1.Commands.Carts.DeleteCart
{
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, Unit>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public DeleteCartHandler(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            ICurrentUser currentUser,
            IApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();

            var cartItem = await GetAndValidateCartItemAsync(request.CartItemId, cancellationToken);

            var cart = await GetAndValidateCartAsync(cartItem.CartId, userId, cancellationToken);

            _cartItemRepository.Remove(cartItem);

            var hasRemainingItems = await _cartItemRepository.AnyAsync(
                ci => ci.CartId == cart.Id && ci.Id != cartItem.Id,
                cancellationToken);

            if (!hasRemainingItems)
            {
                _cartRepository.Remove(cart);
            }
            else
            {
                cart.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private Guid? GetCurrentUserId()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? null : userId;
        }

        private async Task<CartItem> GetAndValidateCartItemAsync(int cartItemId, CancellationToken cancellationToken)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId, cancellationToken);

            if (cartItem is null)
            {
                throw new NotFoundException($"CartItem with ID {cartItemId} not found.");
            }

            return cartItem;
        }

        private async Task<Cart> GetAndValidateCartAsync(Guid cartId, Guid? userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == cartId && c.IsActive,
                asNoTracking: false,
                cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            if (cart.UserId.HasValue)
            {
                if (!userId.HasValue)
                {
                    throw new ForbiddenException("Authentication required to modify this cart.");
                }

                if (cart.UserId.Value != userId.Value)
                {
                    throw new ForbiddenException("You do not have permission to modify this cart.");
                }
            }

            return cart;
        }
    }
}
