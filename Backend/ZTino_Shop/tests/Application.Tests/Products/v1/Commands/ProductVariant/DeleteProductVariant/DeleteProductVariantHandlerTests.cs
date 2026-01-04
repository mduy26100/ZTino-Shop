using Application.Features.Products.v1.Commands.ProductVariants.DeleteProductVariant;
using Application.Features.Products.v1.Repositories;
using MediatR;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.v1.Commands.ProductVariant.DeleteProductVariant
{
    public class DeleteProductVariantHandlerTests
    {
        private readonly Mock<IProductVariantRepository> _productVariantRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteProductVariantHandler _handler;

        public DeleteProductVariantHandlerTests()
        {
            _productVariantRepositoryMock = new Mock<IProductVariantRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteProductVariantHandler(
                _productVariantRepositoryMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_DeleteProductVariant_WhenVariantExists()
        {
            var variantId = 1;
            var entity = new ProductVariantEntity { Id = variantId };

            _productVariantRepositoryMock
                .Setup(r => r.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var result = await _handler.Handle(new DeleteProductVariantCommand(variantId), CancellationToken.None);

            _productVariantRepositoryMock.Verify(r => r.Remove(entity), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenVariantNotFound()
        {
            var variantId = 1;

            _productVariantRepositoryMock
                .Setup(r => r.GetByIdAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(ProductVariantEntity));

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new DeleteProductVariantCommand(variantId), CancellationToken.None)
            );

            _productVariantRepositoryMock.Verify(r => r.Remove(It.IsAny<ProductVariantEntity>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
