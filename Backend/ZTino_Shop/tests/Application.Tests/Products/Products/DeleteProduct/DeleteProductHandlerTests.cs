using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Commands.Products.DeleteProduct;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Products.DeleteProduct
{
    public class DeleteProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteProductHandler _handler;

        public DeleteProductHandlerTests()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteProductHandler(
                _productRepositoryMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenExists()
        {
            // Arrange
            int productId = 123;

            var existingProduct = new Product
            {
                Id = productId,
                Name = "Test Product"
            };

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var command = new DeleteProductCommand(productId);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _productRepositoryMock.Verify(r => r.Remove(existingProduct), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            int productId = 456;

            _productRepositoryMock
                .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var command = new DeleteProductCommand(productId);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _productRepositoryMock.Verify(r => r.Remove(It.IsAny<Product>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
