using Application.Common.Interfaces.Identity;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Carts.v1.Commands.Carts.CreateCart;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Carts;
using Domain.Models.Products;

namespace Application.Tests.Carts.v1.Commands.CreateCart
{
    public class CreateCartHandlerTests
    {
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IProductVariantRepository> _productVariantRepository = new();
        private readonly Mock<ICurrentUser> _currentUser = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private CreateCartHandler CreateHandler()
        {
            return new CreateCartHandler(
                _cartRepository.Object,
                _cartItemRepository.Object,
                _productVariantRepository.Object,
                _currentUser.Object,
                _context.Object);
        }

        [Fact]
        public async Task Handle_Should_Create_New_Cart_When_User_Has_No_Cart()
        {
            var userId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _productVariantRepository
                .Setup(x => x.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductVariant
                {
                    Id = variantId,
                    IsActive = true,
                    StockQuantity = 10
                });

            _cartRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Cart?)null);

            _cartItemRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<CartItem, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem?)null);

            var dto = new UpsertCartDto
            {
                ProductVariantId = variantId,
                Quantity = 2
            };

            var command = new CreateCartCommand(dto);
            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, result.CartId);
            Assert.Equal(variantId, result.ProductVariantId);
            Assert.Equal(2, result.Quantity);

            _cartRepository.Verify(
                x => x.AddAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _cartItemRepository.Verify(
                x => x.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _context.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Update_Existing_CartItem_When_Item_Exists()
        {
            var userId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _productVariantRepository
                .Setup(x => x.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductVariant
                {
                    Id = variantId,
                    IsActive = true,
                    StockQuantity = 10
                });

            _cartRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = cartId,
                    UserId = userId,
                    IsActive = true
                });

            _cartItemRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<CartItem, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CartItem
                {
                    CartId = cartId,
                    ProductVariantId = variantId,
                    Quantity = 3
                });

            var dto = new UpsertCartDto
            {
                CartId = cartId,
                ProductVariantId = variantId,
                Quantity = 2
            };

            var command = new CreateCartCommand(dto);
            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(5, result.Quantity);

            _cartItemRepository.Verify(
                x => x.AddAsync(It.IsAny<CartItem>(), It.IsAny<CancellationToken>()),
                Times.Never);

            _context.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Throw_When_Quantity_Exceeds_Stock()
        {
            var userId = Guid.NewGuid();
            var variantId = 1;

            _currentUser.Setup(x => x.UserId).Returns(userId);

            _productVariantRepository
                .Setup(x => x.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductVariant
                {
                    Id = variantId,
                    IsActive = true,
                    StockQuantity = 2
                });

            _cartRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Cart, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    IsActive = true
                });

            _cartItemRepository
                .Setup(x => x.FindOneAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<CartItem, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((CartItem?)null);

            var dto = new UpsertCartDto
            {
                ProductVariantId = variantId,
                Quantity = 5
            };

            var command = new CreateCartCommand(dto);
            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }
    }
}