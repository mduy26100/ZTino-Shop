using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Carts;
using Domain.Models.Products;

namespace Application.Features.Carts.v1.Commands.Carts.CreateCart
{
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, UpsertCartResponseDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public CreateCartHandler(
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

        public async Task<UpsertCartResponseDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = GetCurrentUserId();

            var variant = await GetAndValidateProductVariantAsync(dto.ProductVariantId, cancellationToken);

            var cart = await GetOrCreateCartAsync(dto.CartId, userId, cancellationToken);

            var (totalQuantity, message) = await AddToCartItemAsync(
                cart.Id, 
                variant, 
                dto.Quantity, 
                cancellationToken);

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            return new UpsertCartResponseDto
            {
                CartId = cart.Id,
                ProductVariantId = dto.ProductVariantId,
                Quantity = totalQuantity,
                Message = message
            };
        }

        private Guid? GetCurrentUserId()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? null : userId;
        }

        private async Task<ProductVariant> GetAndValidateProductVariantAsync(
            int productVariantId,
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

            return variant;
        }

        private async Task<Cart> GetOrCreateCartAsync(Guid? cartId, Guid? userId, CancellationToken cancellationToken)
        {
            if (cartId.HasValue && cartId.Value != Guid.Empty)
            {
                return await GetExistingCartAsync(cartId.Value, userId, cancellationToken);
            }

            if (userId.HasValue)
            {
                return await GetOrCreateUserCartAsync(userId.Value, cancellationToken);
            }

            return await CreateNewCartAsync(null, cancellationToken);
        }

        private async Task<Cart> GetExistingCartAsync(Guid cartId, Guid? userId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == cartId && c.IsActive,
                asNoTracking: false,
                cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            if (userId.HasValue && !cart.UserId.HasValue)
            {
                cart.UserId = userId;
            }

            return cart;
        }

        private async Task<Cart> GetOrCreateUserCartAsync(Guid userId, CancellationToken cancellationToken)
        {
            var existingCart = await _cartRepository.FindOneAsync(
                c => c.UserId == userId && c.IsActive,
                asNoTracking: false,
                cancellationToken);

            if (existingCart is not null)
            {
                return existingCart;
            }

            return await CreateNewCartAsync(userId, cancellationToken);
        }

        private async Task<Cart> CreateNewCartAsync(Guid? userId, CancellationToken cancellationToken)
        {
            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _cartRepository.AddAsync(cart, cancellationToken);
            return cart;
        }

        private async Task<(int TotalQuantity, string Message)> AddToCartItemAsync(
            Guid cartId,
            ProductVariant variant,
            int addQuantity,
            CancellationToken cancellationToken)
        {
            var existingItem = await _cartItemRepository.FindOneAsync(
                ci => ci.CartId == cartId && ci.ProductVariantId == variant.Id,
                asNoTracking: false,
                cancellationToken);

            int currentQuantity = existingItem?.Quantity ?? 0;
            int newTotalQuantity = currentQuantity + addQuantity;

            if (newTotalQuantity > variant.StockQuantity)
            {
                throw new BusinessRuleException(
                    $"Cannot add {addQuantity} items. " +
                    $"Current cart has {currentQuantity}, requesting total {newTotalQuantity}, " +
                    $"but only {variant.StockQuantity} available in stock.");
            }

            if (existingItem is not null)
            {
                existingItem.Quantity = newTotalQuantity;
                existingItem.AddedAt = DateTime.UtcNow;
                return (newTotalQuantity, $"Cart updated. Total quantity: {newTotalQuantity}.");
            }

            var newItem = new CartItem
            {
                CartId = cartId,
                ProductVariantId = variant.Id,
                Quantity = newTotalQuantity,
                AddedAt = DateTime.UtcNow
            };

            await _cartItemRepository.AddAsync(newItem, cancellationToken);
            return (newTotalQuantity, $"Product added to cart. Quantity: {newTotalQuantity}.");
        }
    }
}
