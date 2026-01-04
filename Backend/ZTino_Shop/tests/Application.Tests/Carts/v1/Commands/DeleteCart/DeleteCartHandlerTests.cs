using Application.Features.Carts.v1.Commands.Carts.DeleteCart;
using Application.Features.Carts.v1.Repositories;
using Domain.Models.Carts;
using MediatR;

namespace Application.Tests.Carts.v1.Commands.DeleteCart
{
    public class DeleteCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock = new();
        private readonly Mock<ICartItemRepository> _cartItemRepositoryMock = new();
        private readonly Mock<ICurrentUser> _currentUserMock = new();
        private readonly Mock<IApplicationDbContext> _contextMock = new();

        private DeleteCartHandler CreateHandler()
        {
            return new DeleteCartHandler(
                _cartRepositoryMock.Object,
                _cartItemRepositoryMock.Object,
                _currentUserMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_Remove_CartItem_And_Cart_When_Last_Item()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = cartId,
                UserId = userId,
                IsActive = true
            };

            var cartItem = new CartItem
            {
                Id = 1,
                CartId = cartId
            };

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            _cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(cartItem.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepositoryMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<CartItem, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = CreateHandler();
            var command = new DeleteCartCommand(cartItem.Id);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);

            _cartItemRepositoryMock.Verify(x => x.Remove(cartItem), Times.Once);
            _cartRepositoryMock.Verify(x => x.Remove(cart), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Remove_CartItem_Only_When_Cart_Has_Other_Items()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = cartId,
                UserId = userId,
                IsActive = true
            };

            var cartItem = new CartItem
            {
                Id = 2,
                CartId = cartId
            };

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            _cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(cartItem.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            _cartItemRepositoryMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<CartItem, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = CreateHandler();
            var command = new DeleteCartCommand(cartItem.Id);

            await handler.Handle(command, CancellationToken.None);

            _cartItemRepositoryMock.Verify(x => x.Remove(cartItem), Times.Once);
            _cartRepositoryMock.Verify(x => x.Remove(It.IsAny<Cart>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_CartItem_Not_Exist()
        {
            _cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem?)null);

            var handler = CreateHandler();
            var command = new DeleteCartCommand(99);

            await Assert.ThrowsAsync<NotFoundException>(
                () => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_Forbidden_When_User_Not_Owner()
        {
            var ownerId = Guid.NewGuid();
            var currentUserId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            var cart = new Cart
            {
                Id = cartId,
                UserId = ownerId,
                IsActive = true
            };

            var cartItem = new CartItem
            {
                Id = 3,
                CartId = cartId
            };

            _currentUserMock.Setup(x => x.UserId).Returns(currentUserId);

            _cartItemRepositoryMock
                .Setup(x => x.GetByIdAsync(cartItem.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(cart);

            var handler = CreateHandler();
            var command = new DeleteCartCommand(cartItem.Id);

            await Assert.ThrowsAsync<ForbiddenException>(
                () => handler.Handle(command, CancellationToken.None));
        }
    }
}