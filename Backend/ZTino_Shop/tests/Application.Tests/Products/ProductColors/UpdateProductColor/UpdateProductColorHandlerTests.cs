using Application.Features.Products.v1.Commands.ProductColor.UpdateProductColor;
using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.ProductColors.UpdateProductColor
{
    public class UpdateProductColorHandlerTests
    {
        private readonly Mock<IProductColorRepository> _productColorRepoMock = new();
        private readonly Mock<IProductRepository> _productRepoMock = new();
        private readonly Mock<IColorRepository> _colorRepoMock = new();
        private readonly Mock<IApplicationDbContext> _contextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private UpdateProductColorHandler CreateHandler()
        {
            return new UpdateProductColorHandler(
                _productColorRepoMock.Object,
                _productRepoMock.Object,
                _colorRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ProductColorNotFound_ShouldThrowNotFoundException()
        {
            var dto = new UpsertProductColorDto
            {
                Id = 1,
                ProductId = 1,
                ColorId = 1
            };

            var command = new UpdateProductColorCommand(dto);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductColor?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductNotFound_ShouldThrowNotFoundException()
        {
            var dto = new UpsertProductColorDto
            {
                Id = 1,
                ProductId = 2,
                ColorId = 1
            };

            var command = new UpdateProductColorCommand(dto);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductColor());

            _productRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ColorNotFound_ShouldThrowNotFoundException()
        {
            var dto = new UpsertProductColorDto
            {
                Id = 1,
                ProductId = 1,
                ColorId = 2
            };

            var command = new UpdateProductColorCommand(dto);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductColor());

            _productRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _colorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Color, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductColorConflict_ShouldThrowConflictException()
        {
            var dto = new UpsertProductColorDto
            {
                Id = 1,
                ProductId = 1,
                ColorId = 1
            };

            var command = new UpdateProductColorCommand(dto);

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ProductColor { Id = 1 });

            _productRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _colorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Color, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productColorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductColor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<ConflictException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidRequest_ShouldUpdateProductColorSuccessfully()
        {
            var dto = new UpsertProductColorDto
            {
                Id = 1,
                ProductId = 1,
                ColorId = 1
            };

            var command = new UpdateProductColorCommand(dto);

            var entity = new ProductColor
            {
                Id = 1,
                ProductId = 1,
                ColorId = 1
            };

            _productColorRepoMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _productRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _colorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<Color, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productColorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductColor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map(dto, entity));

            _mapperMock
                .Setup(x => x.Map<UpsertProductColorDto>(entity))
                .Returns(dto);

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto.ProductId, result.ProductId);
            Assert.Equal(dto.ColorId, result.ColorId);

            _productColorRepoMock.Verify(x => x.Update(entity), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}