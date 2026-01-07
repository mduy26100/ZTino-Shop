using Application.Features.Carts.v1.Repositories;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Carts;
using Domain.Models.Orders;
using Domain.Models.Products;

namespace Application.Features.Orders.v1.Commands.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductVariantRepository _productVariantRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductVariantRepository productVariantRepository,
            ICurrentUser currentUser,
            IApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productVariantRepository = productVariantRepository;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CreateOrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = GetCurrentUserId();

            var cart = await GetAndValidateCartAsync(dto.CartId, userId, cancellationToken);
            var cartItems = await GetCartItemsAsync(cart.Id, dto.SelectedCartItemIds, cancellationToken);
            var variantsWithQuantity = await ValidateStockAsync(cartItems, cancellationToken);

            var order = Order.Create(
                userId,
                dto.CustomerName,
                dto.CustomerPhone,
                dto.CustomerEmail,
                dto.Note,
                variantsWithQuantity);

            order.SetShippingAddress(
                dto.ShippingAddress.RecipientName,
                dto.ShippingAddress.PhoneNumber,
                dto.ShippingAddress.Street,
                dto.ShippingAddress.Ward,
                dto.ShippingAddress.District,
                dto.ShippingAddress.City);

            await _orderRepository.AddAsync(order, cancellationToken);

            DeductStock(variantsWithQuantity);
            await CleanupCartAsync(cart, cartItems, userId, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateOrderResponseDto
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                PaymentMethod = order.PaymentMethod,
                Message = "Order created successfully."
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
                cancellationToken)
                ?? throw new NotFoundException($"Cart with ID {cartId} not found.");

            ValidateCartOwnership(cart, userId);
            return cart;
        }

        private static void ValidateCartOwnership(Cart cart, Guid? userId)
        {
            if (userId is null && cart.UserId.HasValue)
            {
                throw new ForbiddenException("This cart belongs to a registered user. Please login to continue.");
            }

            if (userId.HasValue)
            {
                if (cart.UserId.HasValue && cart.UserId.Value != userId.Value)
                {
                    throw new ForbiddenException("You do not have permission to order from this cart.");
                }

                if (!cart.UserId.HasValue)
                {
                    cart.UserId = userId;
                }
            }
        }

        private async Task<List<CartItem>> GetCartItemsAsync(
            Guid cartId,
            List<int> selectedIds,
            CancellationToken cancellationToken)
        {
            if (selectedIds is null || selectedIds.Count == 0)
            {
                throw new BusinessRuleException("Please select at least one item to order.");
            }

            var items = (await _cartItemRepository.FindAsync(
                ci => ci.CartId == cartId && selectedIds.Contains(ci.Id),
                asNoTracking: false,
                cancellationToken)).ToList();

            if (items.Count != selectedIds.Count)
            {
                var missing = selectedIds.Except(items.Select(i => i.Id));
                throw new NotFoundException($"Cart items not found: {string.Join(", ", missing)}");
            }

            return items;
        }

        private async Task<List<(ProductVariant, int)>> ValidateStockAsync(
            List<CartItem> cartItems,
            CancellationToken cancellationToken)
        {
            var result = new List<(ProductVariant, int)>();

            foreach (var item in cartItems)
            {
                var variant = await _productVariantRepository.GetWithDetailsForOrderAsync(
                    item.ProductVariantId, cancellationToken)
                    ?? throw new NotFoundException($"Product variant {item.ProductVariantId} not found.");

                if (!variant.IsActive)
                {
                    throw new BusinessRuleException($"Product variant {item.ProductVariantId} is no longer available.");
                }

                if (variant.StockQuantity < item.Quantity)
                {
                    throw new BusinessRuleException(
                        $"Insufficient stock for variant {item.ProductVariantId}. " +
                        $"Requested: {item.Quantity}, Available: {variant.StockQuantity}.");
                }

                result.Add((variant, item.Quantity));
            }

            return result;
        }

        private static void DeductStock(List<(ProductVariant Variant, int Quantity)> items)
        {
            foreach (var (variant, quantity) in items)
            {
                variant.StockQuantity -= quantity;
            }
        }

        private async Task CleanupCartAsync(
            Cart cart,
            List<CartItem> orderedItems,
            Guid? userId,
            CancellationToken cancellationToken)
        {
            _cartItemRepository.RemoveRange(orderedItems);

            var hasRemaining = (await _cartItemRepository.FindAsync(
                ci => ci.CartId == cart.Id,
                asNoTracking: true,
                cancellationToken)).Any();

            if (!hasRemaining && userId is null)
            {
                _cartRepository.Remove(cart);
            }

            cart.UpdatedAt = DateTime.UtcNow;
        }
    }
}