using Application.Features.Carts.v1.Repositories;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Products.v1.Services;
using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Commands.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IInventoryService _inventoryService;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public CreateOrderHandler(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IInventoryService inventoryService,
            ICurrentUser currentUser,
            IApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _inventoryService = inventoryService;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CreateOrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = _currentUser.UserId == Guid.Empty ? null : (Guid?)_currentUser.UserId;

            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == dto.CartId && c.IsActive,
                asNoTracking: false,
                cancellationToken)
                ?? throw new NotFoundException($"Cart with ID {dto.CartId} not found.");

            cart.ValidateOwnership(userId);
            cart.AssignToUser(userId);

            var selectedItems = cart.CartItems
                .Where(ci => dto.SelectedCartItemIds.Contains(ci.Id))
                .ToList();

            if (selectedItems.Count == 0 || selectedItems.Count != dto.SelectedCartItemIds.Count)
            {
                throw new BusinessRuleException("One or more selected cart items were not found or are invalid.");
            }

            var stockItems = await _inventoryService.PrepareAndValidateStockAsync(selectedItems, cancellationToken);

            var order = Order.Create(
                userId,
                dto.CustomerName,
                dto.CustomerPhone,
                dto.CustomerEmail,
                dto.Note,
                stockItems);

            order.SetShippingAddress(
                dto.ShippingAddress.RecipientName,
                dto.ShippingAddress.PhoneNumber,
                dto.ShippingAddress.Street,
                dto.ShippingAddress.Ward,
                dto.ShippingAddress.District,
                dto.ShippingAddress.City);

            await _orderRepository.AddAsync(order, cancellationToken);

            foreach (var (variant, quantity) in stockItems)
            {
                variant.DeductStock(quantity);
            }

            _cartItemRepository.RemoveRange(selectedItems);
            cart.MarkUpdated();

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
    }
}
