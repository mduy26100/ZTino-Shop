using Application.Features.Carts.v1.Repositories;
using Application.Features.Orders.v1.Commands.Orders.CreateOrder;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Products.v1.Services;
using Domain.Models.Carts;
using Domain.Models.Orders;
using Domain.Models.Products;

namespace Application.Tests.Orders.v1.Commands.CreateOrder
{
    public class CreateOrderHandlerTests
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<ICartRepository> _cartRepo = new();
        private readonly Mock<ICartItemRepository> _cartItemRepo = new();
        private readonly Mock<IInventoryService> _inventoryService = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();
        private readonly Mock<IDatabaseTransaction> _transaction = new();

        private CreateOrderHandler CreateHandler()
        {
            _context
                .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(_transaction.Object);

            return new CreateOrderHandler(
                _orderRepo.Object,
                _cartRepo.Object,
                _cartItemRepo.Object,
                _inventoryService.Object,
                _currentUser.Object,
                _context.Object
            );
        }

        private static CreateOrderCommand CreateCommand(Guid cartId, int cartItemId)
        {
            return new CreateOrderCommand(new CreateOrderDto
            {
                CartId = cartId,
                SelectedCartItemIds = new List<int> { cartItemId },
                CustomerName = "John",
                CustomerPhone = "0123456789",
                CustomerEmail = "john@test.com",
                Note = "note",
                ShippingAddress = new ShippingAddressDto
                {
                    RecipientName = "John",
                    PhoneNumber = "0123456789",
                    Street = "Street",
                    Ward = "Ward",
                    District = "District",
                    City = "City"
                }
            });
        }

        [Fact]
        public async Task Handle_Should_Create_Order_Successfully()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var cartItemId = 1;

            var variant = new ProductVariant
            {
                Id = 10,
                StockQuantity = 10,
                IsActive = true
            };

            var cart = new Cart
            {
                Id = cartId,
                UserId = userId,
                IsActive = true
            };

            var cartItem = new CartItem
            {
                Id = cartItemId,
                CartId = cartId,
                ProductVariantId = variant.Id,
                Quantity = 2
            };

            cart.CartItems.Add(cartItem);

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _cartRepo.Setup(x =>
                x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _inventoryService.Setup(x =>
                x.PrepareAndValidateStockAsync(It.IsAny<List<CartItem>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<(ProductVariant, int)>
                {
                    (variant, cartItem.Quantity)
                });

            var handler = CreateHandler();

            var result = await handler.Handle(CreateCommand(cartId, cartItemId), CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result.OrderId);
            Assert.Equal(8, variant.StockQuantity);

            _orderRepo.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _cartItemRepo.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<CartItem>>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
            _transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Cart_Not_Found()
        {
            _cartRepo.Setup(x =>
                x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(CreateCommand(Guid.NewGuid(), 1), CancellationToken.None));

            _transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_BusinessRule_When_No_Item_Selected()
        {
            var cart = new Cart { Id = Guid.NewGuid(), IsActive = true };

            _cartRepo.Setup(x =>
                x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = CreateHandler();

            var command = new CreateOrderCommand(new CreateOrderDto
            {
                CartId = cart.Id,
                SelectedCartItemIds = new List<int>(),
                CustomerName = "John",
                CustomerPhone = "0123456789",
                ShippingAddress = new ShippingAddressDto
                {
                    RecipientName = "John",
                    PhoneNumber = "0123456789",
                    Street = "Street",
                    Ward = "Ward",
                    District = "District",
                    City = "City"
                }
            });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));

            _transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_BusinessRule_When_CartItem_Missing()
        {
            var cart = new Cart { Id = Guid.NewGuid(), IsActive = true };

            _cartRepo.Setup(x =>
                x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(CreateCommand(cart.Id, 99), CancellationToken.None));

            _transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_BusinessRule_When_Stock_Not_Enough()
        {
            var cartId = Guid.NewGuid();

            var cart = new Cart { Id = cartId, IsActive = true };

            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cartId,
                ProductVariantId = 20,
                Quantity = 5
            };

            cart.CartItems.Add(cartItem);

            _cartRepo.Setup(x =>
                x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _inventoryService.Setup(x =>
                x.PrepareAndValidateStockAsync(It.IsAny<List<CartItem>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new BusinessRuleException("Stock not enough"));

            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(CreateCommand(cartId, cartItem.Id), CancellationToken.None));

            _transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
