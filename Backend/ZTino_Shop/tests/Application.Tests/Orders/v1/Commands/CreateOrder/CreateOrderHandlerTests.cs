using Application.Features.Orders.v1.Commands.Orders.CreateOrder;
using Application.Features.Orders.v1.DTOs;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Products.v1.Repositories;
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
        private readonly Mock<IProductVariantRepository> _variantRepo = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private CreateOrderHandler CreateHandler()
        {
            return new CreateOrderHandler(
                _orderRepo.Object,
                _cartRepo.Object,
                _cartItemRepo.Object,
                _variantRepo.Object,
                _currentUser.Object,
                _context.Object
            );
        }

        private static CreateOrderCommand CreateValidCommand(Guid cartId, int cartItemId)
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
            var variantId = 10;

            var cart = new Cart { Id = cartId, UserId = userId, IsActive = true };
            var cartItem = new CartItem
            {
                Id = cartItemId,
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 2
            };
            var variant = new ProductVariant
            {
                Id = variantId,
                StockQuantity = 10,
                IsActive = true
            };

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem> { cartItem });

            _variantRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<ProductVariant, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem>());

            var handler = CreateHandler();
            var command = CreateValidCommand(cartId, cartItemId);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result.OrderId);
            Assert.Equal(8, variant.StockQuantity);

            _orderRepo.Verify(x => x.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _cartItemRepo.Verify(x => x.RemoveRange(It.IsAny<IEnumerable<CartItem>>()), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_Cart_Not_Exist()
        {
            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            var handler = CreateHandler();
            var command = CreateValidCommand(Guid.NewGuid(), 1);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_BusinessRule_When_No_CartItem_Selected()
        {
            var cartId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = cartId,
                IsActive = true
            };

            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = CreateHandler();

            var command = new CreateOrderCommand(new CreateOrderDto
            {
                CartId = cartId,
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
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_CartItem_Missing()
        {
            var cart = new Cart { Id = Guid.NewGuid(), IsActive = true };

            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem>());

            var handler = CreateHandler();
            var command = CreateValidCommand(cart.Id, 99);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_BusinessRule_When_Stock_Not_Enough()
        {
            var cartId = Guid.NewGuid();
            var variantId = 20;

            var cart = new Cart { Id = cartId, IsActive = true };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 5
            };
            var variant = new ProductVariant
            {
                Id = variantId,
                StockQuantity = 2,
                IsActive = true
            };

            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem> { cartItem });

            _variantRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<ProductVariant, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            var handler = CreateHandler();
            var command = CreateValidCommand(cartId, cartItem.Id);

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Remove_Cart_When_Guest_And_No_Items_Left()
        {
            var cartId = Guid.NewGuid();
            var variantId = 30;

            var cart = new Cart { Id = cartId, IsActive = true };
            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 1
            };
            var variant = new ProductVariant
            {
                Id = variantId,
                StockQuantity = 5,
                IsActive = true
            };

            _currentUser.Setup(x => x.UserId).Returns(Guid.Empty);

            _cartRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem> { cartItem });

            _cartItemRepo.Setup(x =>
                    x.FindAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<CartItem>());

            _variantRepo.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<ProductVariant, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

            var handler = CreateHandler();
            var command = CreateValidCommand(cartId, cartItem.Id);

            await handler.Handle(command, CancellationToken.None);

            _cartRepo.Verify(x => x.Remove(cart), Times.Once);
        }
    }
}