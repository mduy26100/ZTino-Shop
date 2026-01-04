using Application.Common.Interfaces.Identity;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Queries.GetCartById;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Carts.v1.Services;
using Domain.Models.Carts;

namespace Application.Tests.Carts.v1.Queries.GetCartById
{
    public class GetCartByIdHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly Mock<ICartQueryService> _cartQueryServiceMock;
        private readonly Mock<ICurrentUser> _currentUserMock;
        private readonly GetCartByIdHandler _handler;

        public GetCartByIdHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartQueryServiceMock = new Mock<ICartQueryService>();
            _currentUserMock = new Mock<ICurrentUser>();

            _handler = new GetCartByIdHandler(
                _cartRepositoryMock.Object,
                _cartQueryServiceMock.Object,
                _currentUserMock.Object);
        }

        [Fact]
        public async Task Handle_UserLoggedIn_WithCartId_ReturnsCart()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = userId,
                    IsActive = true
                });

            var cartDto = new CartDto { CartId = cartId };

            _cartQueryServiceMock
                .Setup(x => x.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartDto);

            var query = new GetCartByIdQuery(cartId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(cartId, result.CartId);
        }

        [Fact]
        public async Task Handle_UserLoggedIn_WithoutCartId_ReturnsUserCart()
        {
            var userId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            var cartDto = new CartDto { CartId = Guid.NewGuid() };

            _cartQueryServiceMock
                .Setup(x => x.GetCartByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartDto);

            var query = new GetCartByIdQuery(null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(cartDto.CartId, result.CartId);
        }

        [Fact]
        public async Task Handle_Guest_WithValidCartId_ReturnsCart()
        {
            var cartId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(Guid.Empty);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = null,
                    IsActive = true
                });

            var cartDto = new CartDto { CartId = cartId };

            _cartQueryServiceMock
                .Setup(x => x.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cartDto);

            var query = new GetCartByIdQuery(cartId);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(cartId, result.CartId);
        }

        [Fact]
        public async Task Handle_Guest_WithoutCartId_ReturnsEmptyCart()
        {
            _currentUserMock.Setup(x => x.UserId).Returns(Guid.Empty);

            var query = new GetCartByIdQuery(null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(Guid.Empty, result.CartId);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalItems);
            Assert.Equal(0, result.TotalPrice);
        }

        [Fact]
        public async Task Handle_CartNotFound_ThrowsNotFoundException()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            var query = new GetCartByIdQuery(cartId);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UserAccessingOtherUserCart_ThrowsForbiddenException()
        {
            var userId = Guid.NewGuid();
            var otherUserId = Guid.NewGuid();
            var cartId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(userId);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = otherUserId,
                    IsActive = true
                });

            var query = new GetCartByIdQuery(cartId);

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_GuestAccessingUserCart_ThrowsForbiddenException()
        {
            var cartId = Guid.NewGuid();

            _currentUserMock.Setup(x => x.UserId).Returns(Guid.Empty);

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = Guid.NewGuid(),
                    IsActive = true
                });

            var query = new GetCartByIdQuery(cartId);

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }
    }
}