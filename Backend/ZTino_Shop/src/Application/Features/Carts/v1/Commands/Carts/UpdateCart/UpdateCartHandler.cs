using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Carts;
using Domain.Models.Products;

namespace Application.Features.Carts.v1.Commands.Carts.UpdateCart
{
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpsertCartResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public UpdateCartHandler(
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductVariantRepository productVariantRepository,
            ICurrentUser currentUser,
            IApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productVariantRepository = productVariantRepository;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<UpsertCartResponseDto> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = GetCurrentUserId();

            var cart = await GetAndValidateCartAsync(dto.CartId!.Value, userId, cancellationToken);

            var cartItem = await GetCartItemAsync(cart.Id, dto.ProductVariantId, cancellationToken);

            string message;
            int finalQuantity;

            if (dto.Quantity == 0)
            {
                _cartItemRepository.Remove(cartItem);
                finalQuantity = 0;
                message = "Item removed from cart successfully.";
            }
            else
            {
                var variant = await GetAndValidateProductVariantAsync(dto.ProductVariantId, dto.Quantity, cancellationToken);
                cartItem.Quantity = dto.Quantity;
                cartItem.AddedAt = DateTime.UtcNow;
                finalQuantity = dto.Quantity;
                message = $"Cart item quantity updated to {finalQuantity}.";
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return new UpsertCartResponseDto
            {
                CartId = cart.Id,
                ProductVariantId = dto.ProductVariantId,
                Quantity = finalQuantity,
                Message = message
            };
        }

        private Guid? GetCurrentUserId()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? null : userId;
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

            // Security check: If cart belongs to a user, require authentication and ownership
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

        private async Task<CartItem> GetCartItemAsync(Guid cartId, int productVariantId, CancellationToken cancellationToken)
        {
            var cartItem = await _cartItemRepository.FindOneAsync(
                ci => ci.CartId == cartId && ci.ProductVariantId == productVariantId,
                asNoTracking: false,
                cancellationToken);

            if (cartItem is null)
            {
                throw new NotFoundException($"Cart item with ProductVariantId {productVariantId} not found in cart.");
            }

            return cartItem;
        }

        private async Task<ProductVariant> GetAndValidateProductVariantAsync(
            int productVariantId,
            int requestedQuantity,
            CancellationToken cancellationToken)
        {
            var variant = await _productVariantRepository.GetByIdAsync(productVariantId, cancellationToken);

            if (variant is null)
            {
                throw new NotFoundException($"ProductVariant with ID {productVariantId} not found.");
            }

            if (!variant.IsActive)
            {
                throw new BusinessRuleException($"ProductVariant with ID {productVariantId} is not available for sale.");
            }

            if (variant.StockQuantity <= 0)
            {
                throw new BusinessRuleException($"ProductVariant with ID {productVariantId} is out of stock.");
            }

            if (requestedQuantity > variant.StockQuantity)
            {
                throw new BusinessRuleException(
                    $"Requested quantity ({requestedQuantity}) exceeds available stock ({variant.StockQuantity}).");
            }

            return variant;
        }
    }
}
