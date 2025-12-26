using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Commands.ProductImages.DeleteProductImage;
using Application.Features.Products.Repositories;
using Domain.Models.Products;
using MediatR;

namespace Application.Tests.Products.ProductImages.DeleteProductImage
{
    public class DeleteProductImageHandlerTests
    {
        private readonly Mock<IProductImageRepository> _repoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteProductImageHandler _handler;

        public DeleteProductImageHandlerTests()
        {
            _repoMock = new Mock<IProductImageRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteProductImageHandler(
                _repoMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenImageNotFound()
        {
            _repoMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductImage?)null);

            var command = new DeleteProductImageCommand(1);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldDeleteNonMainImage()
        {
            var image = new ProductImage
            {
                Id = 1,
                ProductVariantId = 10,
                IsMain = false
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(image.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(image);

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var command = new DeleteProductImageCommand(image.Id);

            var result = await _handler.Handle(command, CancellationToken.None);

            _repoMock.Verify(x => x.Remove(image), Times.Once);
            _repoMock.Verify(x => x.Update(It.IsAny<ProductImage>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(Unit.Value, result);
        }

        [Fact]
        public async Task Handle_ShouldReassignMain_WhenDeletingMainImage_WithSibling()
        {
            var mainImage = new ProductImage
            {
                Id = 1,
                ProductVariantId = 10,
                IsMain = true
            };

            var sibling = new ProductImage
            {
                Id = 2,
                ProductVariantId = 10,
                IsMain = false
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(mainImage.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mainImage);

            _repoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage> { sibling });

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var command = new DeleteProductImageCommand(mainImage.Id);

            await _handler.Handle(command, CancellationToken.None);

            _repoMock.Verify(x => x.Remove(mainImage), Times.Once);
            _repoMock.Verify(x => x.Update(It.Is<ProductImage>(p => p.Id == sibling.Id && p.IsMain)), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ShouldDeleteMainImage_WhenNoSiblingExists()
        {
            var mainImage = new ProductImage
            {
                Id = 1,
                ProductVariantId = 10,
                IsMain = true
            };

            _repoMock
                .Setup(x => x.GetByIdAsync(mainImage.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mainImage);

            _repoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            _contextMock
                .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var command = new DeleteProductImageCommand(mainImage.Id);

            await _handler.Handle(command, CancellationToken.None);

            _repoMock.Verify(x => x.Remove(mainImage), Times.Once);
            _repoMock.Verify(x => x.Update(It.IsAny<ProductImage>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
