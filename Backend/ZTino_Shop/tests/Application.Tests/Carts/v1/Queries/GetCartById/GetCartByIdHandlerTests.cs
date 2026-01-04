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
        private readonly GetCartByIdHandler _handler;

        public GetCartByIdHandlerTests()
        {
            _cartRepositoryMock = new Mock<ICartRepository>();
            _cartQueryServiceMock = new Mock<ICartQueryService>();

            _handler = new GetCartByIdHandler(
                _cartRepositoryMock.Object,
                _cartQueryServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidGuestCart_ReturnsCart()
        {
            var cartId = Guid.NewGuid();

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

            var result = await _handler.Handle(new GetCartByIdQuery(cartId), CancellationToken.None);

            Assert.Equal(cartId, result.CartId);
        }

        [Fact]
        public async Task Handle_CartNotFound_ThrowsNotFoundException()
        {
            var cartId = Guid.NewGuid();

            _cartRepositoryMock
                .Setup(x => x.FindOneAsync(
                    It.IsAny<Expression<Func<Cart, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new GetCartByIdQuery(cartId), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_UserOwnedCart_ThrowsForbiddenException()
        {
            var cartId = Guid.NewGuid();

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

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                _handler.Handle(new GetCartByIdQuery(cartId), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidGuestCart_ButQueryReturnsNull_ReturnsEmptyCart()
        {
            var cartId = Guid.NewGuid();

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

            _cartQueryServiceMock
                .Setup(x => x.GetCartByIdAsync(cartId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartDto?)null);

            var result = await _handler.Handle(new GetCartByIdQuery(cartId), CancellationToken.None);

            Assert.Equal(Guid.Empty, result.CartId);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalItems);
            Assert.Equal(0, result.TotalPrice);
        }
    }
}