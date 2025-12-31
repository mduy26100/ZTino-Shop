using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.v1.Commands.Sizes.DeleteSize;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using MediatR;

namespace Application.Tests.Products.Sizes.DeleteSize
{
    public class DeleteSizeHandlerTests
    {
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<IProductVariantRepository> _productVariantRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteSizeHandler _handler;

        public DeleteSizeHandlerTests()
        {
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _productVariantRepositoryMock = new Mock<IProductVariantRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteSizeHandler(
                _sizeRepositoryMock.Object,
                _productVariantRepositoryMock.Object,
                _contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_DeleteSize_WhenSizeExistsAndNotInUse()
        {
            var sizeId = 1;
            var size = new Size { Id = sizeId, Name = "Large" };
            var command = new DeleteSizeCommand(sizeId);

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(sizeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(size);

            _productVariantRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Models.Products.ProductVariant, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(Unit.Value, result);

            _sizeRepositoryMock.Verify(x => x.Remove(size), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenSizeNotFound()
        {
            var sizeId = 999;
            var command = new DeleteSizeCommand(sizeId);

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(sizeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Size?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Size with Id {sizeId} not found.", exception.Message);

            _sizeRepositoryMock.Verify(x => x.Remove(It.IsAny<Size>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenSizeIsInUse()
        {
            var sizeId = 1;
            var size = new Size { Id = sizeId, Name = "Medium" };
            var command = new DeleteSizeCommand(sizeId);

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(sizeId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(size);

            _productVariantRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Domain.Models.Products.ProductVariant, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal(
                "Cannot delete size as it is associated with existing product variants.",
                exception.Message);

            _sizeRepositoryMock.Verify(x => x.Remove(It.IsAny<Size>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
