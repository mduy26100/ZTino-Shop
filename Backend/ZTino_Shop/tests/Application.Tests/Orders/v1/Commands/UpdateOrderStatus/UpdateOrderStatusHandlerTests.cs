using Application.Features.Orders.v1.Commands.Orders.UpdateOrderStatus;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Domain.Constants;
using Domain.Models.Orders;

namespace Application.Tests.Orders.v1.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IOrderStatusHistoryRepository> _orderStatusHistoryRepository = new();
        private readonly Mock<IOrderStatusService> _orderStatusService = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private UpdateOrderStatusHandler CreateHandler()
        {
            return new UpdateOrderStatusHandler(
                _orderRepository.Object,
                _orderStatusHistoryRepository.Object,
                _orderStatusService.Object,
                _currentUser.Object,
                _context.Object
            );
        }

        private static Order CreateOrder(string status)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                OrderCode = "ORD-20260105100413-A10CE6",
                Status = status,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                PaymentStatus = PaymentStatus.Pending
            };
        }

        [Fact]
        public async Task Handle_Should_Update_Order_Status_Successfully()
        {
            var order = CreateOrder(OrderStatus.Pending);

            _orderRepository
                .Setup(x => x.GetWithDetailsForUpdateAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var command = new UpdateOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Confirmed
            });

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatus.Confirmed, result.Status);

            _orderStatusService.Verify(
                x => x.ValidateStatusTransition(OrderStatus.Pending, OrderStatus.Confirmed),
                Times.Once);

            _orderStatusService.Verify(
                x => x.ProcessStatusChangeAsync(order, OrderStatus.Confirmed, It.IsAny<CancellationToken>()),
                Times.Once);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Set_CancelReason_When_Order_Is_Cancelled()
        {
            var order = CreateOrder(OrderStatus.Confirmed);

            _orderRepository
                .Setup(x => x.GetWithDetailsForUpdateAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var command = new UpdateOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Cancelled,
                CancelReason = "Admin cancelled"
            });

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatus.Cancelled, result.Status);
            Assert.Equal("Admin cancelled", order.CancelReason);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Order_Not_Found()
        {
            _orderRepository
                .Setup(x => x.GetWithDetailsForUpdateAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var command = new UpdateOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = Guid.NewGuid(),
                NewStatus = OrderStatus.Confirmed
            });

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Status_Transition_Is_Invalid()
        {
            var order = CreateOrder(OrderStatus.Delivered);

            _orderRepository
                .Setup(x => x.GetWithDetailsForUpdateAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderStatusService
                .Setup(x => x.ValidateStatusTransition(order.Status, OrderStatus.Pending))
                .Throws(new BusinessRuleException("Invalid transition"));

            var command = new UpdateOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Pending
            });

            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}