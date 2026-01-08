using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Domain.Constants;
using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, UpdateOrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderStatusHistoryRepository _orderStatusHistoryRepository;
        private readonly IOrderStatusService _orderStatusService;
        private readonly ICurrentUser _currentUser;
        private readonly IApplicationDbContext _context;

        public UpdateOrderStatusHandler(
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

        public async Task<UpdateOrderResponseDto> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // 1. Get order with all related data
            var order = await _orderRepository.GetWithDetailsForUpdateAsync(dto.OrderId, cancellationToken)
                ?? throw new NotFoundException($"Order with ID {dto.OrderId} not found.");

            // 2. Validate status transition
            _orderStatusService.ValidateStatusTransition(order.Status, dto.NewStatus);

            // 3. Update order status
            order.Status = dto.NewStatus;
            order.UpdatedAt = DateTime.UtcNow;

            if (dto.NewStatus == OrderStatus.Cancelled)
            {
                order.CancelReason = dto.CancelReason;
            }

            // 4. Add status history
            await AddStatusHistoryAsync(order.Id, dto.NewStatus, dto.Note, cancellationToken);

            // 5. Process status-specific actions (delivery, cancellation, return)
            await _orderStatusService.ProcessStatusChangeAsync(order, dto.NewStatus, cancellationToken);

            // 6. Save all changes
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResponseDto
            {
                OrderId = order.Id,
                OrderCode = order.OrderCode,
                Status = order.Status,
                PaymentStatus = order.PaymentStatus,
                Message = $"Order status updated to {dto.NewStatus} successfully."
            };
        }

        private async Task AddStatusHistoryAsync(Guid orderId, string status, string? note, CancellationToken cancellationToken)
        {
            var history = new OrderStatusHistory
            {
                OrderId = orderId,
                Status = status,
                Note = note ?? $"Order status changed to {status}.",
                ChangedBy = GetCurrentUserName(),
                CreatedAt = DateTime.UtcNow
            };

            await _orderStatusHistoryRepository.AddAsync(history, cancellationToken);
        }

        private string GetCurrentUserName()
        {
            var userId = _currentUser.UserId;
            return userId == Guid.Empty ? "System" : userId.ToString();
        }
    }
}
