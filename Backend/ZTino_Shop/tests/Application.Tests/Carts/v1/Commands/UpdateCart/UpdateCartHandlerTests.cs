using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Carts.v1.Commands.Carts.UpdateCart;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Carts;
using Domain.Models.Products;

namespace Application.Tests.Carts.v1.Commands.UpdateCart
{
    public class UpdateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IProductVariantRepository> _productVariantRepository = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private UpdateCartHandler CreateHandler()
        {
            return new UpdateCartHandler(
                _cartRepository.Object,
                _cartItemRepository.Object,
                _productVariantRepository.Object,
                _currentUser.Object,
                _context.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Quantity_When_Valid_Request()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _cartRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = userId,
                    IsActive = true
                });

            _cartItemRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartItem
                {
                    CartId = cartId,
                    ProductVariantId = variantId,
                    Quantity = 1
                });

            _productVariantRepository.Setup(x =>
                    x.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductVariant
                {
                    Id = variantId,
                    IsActive = true,
                    StockQuantity = 10
                });

            var dto = new UpsertCartDto
            {
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 5
            };

            var command = new UpdateCartCommand(dto);
            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(cartId, result.CartId);
            Assert.Equal(variantId, result.ProductVariantId);
            Assert.Equal(5, result.Quantity);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Remove_Item_When_Quantity_Is_Zero()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _cartRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = userId,
                    IsActive = true
                });

            var cartItem = new CartItem
            {
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 3
            };

            _cartItemRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartItem);

            var dto = new UpsertCartDto
            {
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 0
            };

            var command = new UpdateCartCommand(dto);
            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(0, result.Quantity);

            _cartItemRepository.Verify(x => x.Remove(cartItem), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_User_Not_Owner()
        {
            var cartId = Guid.NewGuid();
            var cartOwnerId = Guid.NewGuid();
            var currentUserId = Guid.NewGuid();

            _currentUser.Setup(x => x.UserId).Returns(currentUserId);

            _cartRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = cartOwnerId,
                    IsActive = true
                });

            var dto = new UpsertCartDto
            {
                CartId = cartId,
                ProductVariantId = 1,
                Quantity = 1
            };

            var command = new UpdateCartCommand(dto);
            var handler = CreateHandler();

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Quantity_Exceeds_Stock()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _cartRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<Cart, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = userId,
                    IsActive = true
                });

            _cartItemRepository.Setup(x =>
                    x.FindOneAsync(It.IsAny<Expression<Func<CartItem, bool>>>(), false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartItem
                {
                    CartId = cartId,
                    ProductVariantId = variantId,
                    Quantity = 1
                });

            _productVariantRepository.Setup(x =>
                    x.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductVariant
                {
                    Id = variantId,
                    IsActive = true,
                    StockQuantity = 2
                });

            var dto = new UpsertCartDto
            {
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 5
            };

            var command = new UpdateCartCommand(dto);
            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}