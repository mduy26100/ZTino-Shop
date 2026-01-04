using Application.Features.Products.v1.Commands.ProductColor.CreateProductColor;
using Application.Features.Products.v1.DTOs.ProductColor;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.ProductColors.CreateProductColor
{
    public class CreateProductColorHandlerTests
    {
        private readonly Mock<IProductColorRepository> _productColorRepoMock = new();
        private readonly Mock<IProductRepository> _productRepoMock = new();
        private readonly Mock<IColorRepository> _colorRepoMock = new();
        private readonly Mock<IApplicationDbContext> _contextMock = new();
        private readonly Mock<IMapper> _mapperMock = new();

        private CreateProductColorHandler CreateHandler()
        {
            return new CreateProductColorHandler(
                _productColorRepoMock.Object,
                _productRepoMock.Object,
                _colorRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ProductNotFound_ShouldThrowNotFoundException()
        {
            var command = new CreateProductColorCommand(new UpsertProductColorDto
            {
                ProductId = 1,
                ColorId = 1
            });

            _productRepoMock
                .Setup(x => x.GetByIdAsync(command.Dto.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ColorNotFound_ShouldThrowNotFoundException()
        {
            var command = new CreateProductColorCommand(new UpsertProductColorDto
            {
                ProductId = 1,
                ColorId = 2
            });

            _productRepoMock
                .Setup(x => x.GetByIdAsync(command.Dto.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product());

            _colorRepoMock
                .Setup(x => x.GetByIdAsync(command.Dto.ColorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Color?)null);

            var handler = CreateHandler();

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductColorAlreadyExists_ShouldThrowConflictException()
        {
            var command = new CreateProductColorCommand(new UpsertProductColorDto
            {
                ProductId = 1,
                ColorId = 1
            });

            _productRepoMock
                .Setup(x => x.GetByIdAsync(command.Dto.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product());

            _colorRepoMock
                .Setup(x => x.GetByIdAsync(command.Dto.ColorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Color());

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
        public async Task Handle_ValidRequest_ShouldCreateProductColorSuccessfully()
        {
            var dto = new UpsertProductColorDto
            {
                ProductId = 1,
                ColorId = 1
            };

            var command = new CreateProductColorCommand(dto);

            var entity = new ProductColor
            {
                ProductId = 1,
                ColorId = 1
            };

            _productRepoMock
                .Setup(x => x.GetByIdAsync(dto.ProductId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Product());

            _colorRepoMock
                .Setup(x => x.GetByIdAsync(dto.ColorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Color());

            _productColorRepoMock
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductColor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map<ProductColor>(dto))
                .Returns(entity);

            _mapperMock
                .Setup(x => x.Map<UpsertProductColorDto>(entity))
                .Returns(dto);

            var handler = CreateHandler();

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto.ProductId, result.ProductId);
            Assert.Equal(dto.ColorId, result.ColorId);

            _productColorRepoMock.Verify(
                x => x.AddAsync(entity, It.IsAny<CancellationToken>()),
                Times.Once);

            _contextMock.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}