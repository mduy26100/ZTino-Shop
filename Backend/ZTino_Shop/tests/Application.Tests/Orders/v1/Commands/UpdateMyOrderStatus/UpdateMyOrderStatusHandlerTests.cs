using Application.Features.Orders.v1.Commands.Orders.UpdateMyOrderStatus;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Domain.Constants;
using Domain.Models.Orders;

namespace Application.Tests.Orders.v1.Commands.UpdateMyOrderStatus
{
    public class UpdateMyOrderStatusHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepository = new();
        private readonly Mock<IOrderStatusHistoryRepository> _orderStatusHistoryRepository = new();
        private readonly Mock<IOrderStatusService> _orderStatusService = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private UpdateMyOrderStatusHandler CreateHandler()
        {
            return new UpdateMyOrderStatusHandler(
                _orderRepository.Object,
                _orderStatusHistoryRepository.Object,
                _orderStatusService.Object,
                _currentUser.Object,
                _context.Object
            );
        }

        private static Order CreateOrder(string status, DateTime? updatedAt = null)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderCode = "ORD-20260107111707-414F73",
                Status = status,
                CreatedAt = DateTime.UtcNow.AddDays(-10),
                UpdatedAt = updatedAt,
                PaymentStatus = PaymentStatus.Pending
            };
        }

        [Fact]
        public async Task Handle_Should_Cancel_Order_When_Status_Is_Pending()
        {
            var userId = Guid.NewGuid();
            var order = CreateOrder(OrderStatus.Pending);
            order.UserId = userId;

            _currentUser.Setup(x => x.UserId).Returns(userId);
            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(order.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Cancelled,
                CancelReason = "Changed my mind"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatus.Cancelled, result.Status);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Cancel_From_Shipping()
        {
            var userId = Guid.NewGuid();
            var order = CreateOrder(OrderStatus.Shipping);
            order.UserId = userId;

            _currentUser.Setup(x => x.UserId).Returns(userId);
            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(order.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Cancelled,
                CancelReason = "Too late"
            });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Deliver_Order_When_Status_Is_Shipping()
        {
            var userId = Guid.NewGuid();
            var order = CreateOrder(OrderStatus.Shipping);
            order.UserId = userId;

            _currentUser.Setup(x => x.UserId).Returns(userId);
            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(order.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Delivered
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatus.Delivered, result.Status);
        }

        [Fact]
        public async Task Handle_Should_Return_Order_When_Delivered_Within_7Days()
        {
            var userId = Guid.NewGuid();
            var order = CreateOrder(OrderStatus.Delivered, DateTime.UtcNow.AddDays(-3));
            order.UserId = userId;

            _currentUser.Setup(x => x.UserId).Returns(userId);
            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(order.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Returned,
                Note = "Defective product"
            });

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(OrderStatus.Returned, result.Status);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Return_After_7Days()
        {
            var userId = Guid.NewGuid();
            var order = CreateOrder(OrderStatus.Delivered, DateTime.UtcNow.AddDays(-8));
            order.UserId = userId;

            _currentUser.Setup(x => x.UserId).Returns(userId);
            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(order.Id, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = order.Id,
                NewStatus = OrderStatus.Returned,
                Note = "Late return"
            });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Order_Not_Found_Or_Not_Owned()
        {
            var userId = Guid.NewGuid();
            _currentUser.Setup(x => x.UserId).Returns(userId);

            _orderRepository.Setup(x =>
                    x.GetByIdAndUserIdAsync(It.IsAny<Guid>(), userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order?)null);

            var handler = CreateHandler();

            var command = new UpdateMyOrderStatusCommand(new UpdateOrderDto
            {
                OrderId = Guid.NewGuid(),
                NewStatus = OrderStatus.Cancelled,
                CancelReason = "Test"
            });

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}