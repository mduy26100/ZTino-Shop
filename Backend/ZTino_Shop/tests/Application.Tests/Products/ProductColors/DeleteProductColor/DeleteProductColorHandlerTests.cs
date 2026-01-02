using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.Commands.ProductColor.DeleteProductColor;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using MediatR;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.ProductColors.DeleteProductColor
{
    public class DeleteProductColorHandlerTests
    {
        private readonly Mock<IProductColorRepository> _productColorRepoMock = new();
        private readonly Mock<IProductImageRepository> _productImageRepoMock = new();
        private readonly Mock<IProductVariantRepository> _productVariantRepoMock = new();
        private readonly Mock<IApplicationDbContext> _contextMock = new();

        private DeleteProductColorHandler CreateHandler()
        {
            return new DeleteProductColorHandler(
                _productColorRepoMock.Object,
                _productImageRepoMock.Object,
                _productVariantRepoMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ProductColorNotFound_ShouldThrowNotFoundException()
        {
            var command = new DeleteProductColorCommand(1);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductColor?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_HasProductVariants_ShouldThrowBusinessRuleException()
        {
            var command = new DeleteProductColorCommand(1);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductColor { Id = 1 });

            _productVariantRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_HasProductImages_ShouldThrowBusinessRuleException()
        {
            var command = new DeleteProductColorCommand(1);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductColor { Id = 1 });

            _productVariantRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _productImageRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldDeleteProductColorSuccessfully()
        {
            var command = new DeleteProductColorCommand(1);

            var entity = new ProductColor { Id = 1 };

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _productVariantRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _productImageRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);

            _productColorRepoMock.Verify(x => x.Remove(entity), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}