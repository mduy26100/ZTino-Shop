using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Domain.Constants;
using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateMyOrderStatus
{
    public class UpdateMyOrderStatusHandler : IRequestHandler<UpdateMyOrderStatusCommand, UpdateOrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusHistoryRepository _orderStatusHistoryRepository;
        private readonly IOrderStatusService _orderStatusService;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        private static readonly Dictionary<string, HashSet<string>> UserAllowedTransitions = new()
        {
            { OrderStatus.Pending, new HashSet<string> { OrderStatus.Cancelled } },
            { OrderStatus.Confirmed, new HashSet<string> { OrderStatus.Cancelled } },
            { OrderStatus.Shipping, new HashSet<string> { OrderStatus.Delivered } },
            { OrderStatus.Delivered, new HashSet<string> { OrderStatus.Returned } },
            { OrderStatus.Cancelled, new HashSet<string>() },
            { OrderStatus.Returned, new HashSet<string>() }
        };

        private const int ReturnWindowDays = 7;

        public UpdateMyOrderStatusHandler(
            IOrderRepository orderRepository,
            IOrderStatusHistoryRepository orderStatusHistoryRepository,
            IOrderStatusService orderStatusService,
            ICurrentUser currentUser,
            IApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _orderStatusHistoryRepository = orderStatusHistoryRepository;
            _orderStatusService = orderStatusService;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<UpdateOrderResponseDto> Handle(
            UpdateMyOrderStatusCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("User must be authenticated to update order status.");
            }

            var order = await _orderRepository.GetByIdAndUserIdAsync(dto.OrderId, userId, cancellationToken)
                ?? throw new NotFoundException($"Order with ID {dto.OrderId} not found or does not belong to you.");

            ValidateUserStatusTransition(order.Status, dto.NewStatus);

            ValidateReturnTimeWindow(order, dto.NewStatus);

            order.Status = dto.NewStatus;
            order.UpdatedAt = DateTime.UtcNow;

            if (dto.NewStatus == OrderStatus.Cancelled)
            {
                order.CancelReason = dto.CancelReason;
            }

            await AddStatusHistoryAsync(order.Id, dto.NewStatus, dto.Note, cancellationToken);

            await _orderStatusService.ProcessStatusChangeAsync(order, dto.NewStatus, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResponseDto
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                Message = GetSuccessMessage(dto.NewStatus)
            };
        }

        private static void ValidateUserStatusTransition(string currentStatus, string newStatus)
        {
            if (currentStatus == newStatus)
            {
                throw new BusinessRuleException($"Order is already in '{currentStatus}' status.");
            }

            if (!UserAllowedTransitions.TryGetValue(currentStatus, out var allowed) || !allowed.Contains(newStatus))
            {
                var errorMessage = currentStatus == OrderStatus.Shipping && newStatus == OrderStatus.Cancelled
                    ? "Cannot cancel order while shipping. Please contact customer support."
                    : $"Cannot transition order from '{currentStatus}' to '{newStatus}'. " +
                      $"Allowed actions: {string.Join(", ", UserAllowedTransitions.GetValueOrDefault(currentStatus, new HashSet<string>()))}";

                throw new BusinessRuleException(errorMessage);
            }
        }

        private static void ValidateReturnTimeWindow(Order order, string newStatus)
        {
            if (newStatus != OrderStatus.Returned)
            {
                return;
            }

            var deliveryDate = order.UpdatedAt ?? order.CreatedAt;
            var daysSinceDelivery = (DateTime.UtcNow - deliveryDate).TotalDays;

            if (daysSinceDelivery > ReturnWindowDays)
            {
                throw new BusinessRuleException(
                    $"Return request is outside the {ReturnWindowDays}-day return window. " +
                    $"Order was delivered {(int)daysSinceDelivery} days ago. " +
                    "Please contact customer support for assistance.");
            }
        }

        private async Task AddStatusHistoryAsync(
            Guid orderId,
            string status,
            string? note,
            CancellationToken cancellationToken)
        {
            var history = new OrderStatusHistory
            {
                OrderId = orderId,
                Status = status,
                Note = note ?? GetDefaultNote(status),
                ChangedBy = GetCurrentUserName(),
                CreatedAt = DateTime.UtcNow
            };

            await _orderStatusHistoryRepository.AddAsync(history, cancellationToken);
        }

        private string GetCurrentUserName()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? "System" : $"Customer:{userId}";
        }

        private static string GetDefaultNote(string status) => status switch
        {
            OrderStatus.Cancelled => "Order cancelled by customer.",
            OrderStatus.Returned => "Return requested by customer.",
            OrderStatus.Delivered => "Delivery confirmed by customer.",
            _ => $"Status changed to {status} by customer."
        };

        private static string GetSuccessMessage(string status) => status switch
        {
            OrderStatus.Cancelled => "Order has been cancelled successfully.",
            OrderStatus.Returned => "Return request has been submitted successfully. Our team will process it shortly.",
            OrderStatus.Delivered => "Thank you for confirming delivery!",
            _ => $"Order status updated to {status} successfully."
        };
    }
}
