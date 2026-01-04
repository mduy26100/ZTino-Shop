using Application.Features.Products.v1.Commands.Colors.DeleteColor;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.Colors.DeleteColor
{
    public class DeleteColorHandlerTests
    {
        private readonly Mock<IColorRepository> _colorRepositoryMock;
        private readonly Mock<IProductColorRepository> _productColorRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteColorHandler _handler;

        public DeleteColorHandlerTests()
        {
            _colorRepositoryMock = new Mock<IColorRepository>();
            _productColorRepositoryMock = new Mock<IProductColorRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteColorHandler(
                _colorRepositoryMock.Object,
                _productColorRepositoryMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteColor_WhenColorExistsAndNoProductAssociations()
        {
            int id = 1;
            var entity = new Color { Id = id, Name = "Red" };

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _productColorRepositoryMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await _handler.Handle(new DeleteColorCommand(id), CancellationToken.None);

            _colorRepositoryMock.Verify(r => r.Remove(entity), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenColorNotFound()
        {
            int id = 999;

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Color?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(new DeleteColorCommand(id), CancellationToken.None)
            );

            _colorRepositoryMock.Verify(r => r.Remove(It.IsAny<Color>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenColorIsAssociatedWithProducts()
        {
            int id = 1;
            var entity = new Color { Id = id, Name = "Blue" };

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _productColorRepositoryMock
                .Setup(r => r.AnyAsync(
                    It.IsAny<Expression<Func<ProductColor, bool>>>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(new DeleteColorCommand(id), CancellationToken.None)
            );

            _colorRepositoryMock.Verify(r => r.Remove(It.IsAny<Color>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}