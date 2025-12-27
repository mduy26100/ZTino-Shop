using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Commands.ProductVariants.UpdateProductVariant;
using Application.Features.Products.DTOs.ProductVariants;
using Application.Features.Products.Repositories;
using Domain.Models.Products;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.ProductVariant.UpdateProductVariant
{
    public class UpdateProductVariantHandlerTests
    {
        private readonly Mock<IProductVariantRepository> _productVariantRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<IColorRepository> _colorRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductVariantHandler _handler;

        public UpdateProductVariantHandlerTests()
        {
            _productVariantRepositoryMock = new Mock<IProductVariantRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _colorRepositoryMock = new Mock<IColorRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateProductVariantHandler(
                _productVariantRepositoryMock.Object,
                _productRepositoryMock.Object,
                _sizeRepositoryMock.Object,
                _colorRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_UpdateProductVariant_WhenAllValid()
        {
            var dto = new UpsertProductVariantDto
            {
                Id = 1,
                ProductId = 1,
                SizeId = 2,
                ColorId = 3,
                StockQuantity = 10,
                Price = 100,
                IsActive = true
            };
            var command = new UpdateProductVariantCommand(dto);
            var entity = new ProductVariantEntity
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                SizeId = dto.SizeId,
                ColorId = dto.ColorId,
                StockQuantity = dto.StockQuantity,
                Price = dto.Price,
                IsActive = dto.IsActive
            };

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(entity);

            _productRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);
            _colorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Color, bool>>>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(true);
            _productVariantRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map(dto, entity));
            _mapperMock.Setup(m => m.Map<UpsertProductVariantDto>(entity)).Returns(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto.Id, result.Id);
            Assert.Equal(dto.ProductId, result.ProductId);
            Assert.Equal(dto.SizeId, result.SizeId);
            Assert.Equal(dto.ColorId, result.ColorId);

            _productVariantRepositoryMock.Verify(r => r.Update(entity), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenVariantNotFound()
        {
            var dto = new UpsertProductVariantDto { Id = 1 };
            var command = new UpdateProductVariantCommand(dto);

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                             .ReturnsAsync(default(ProductVariantEntity));

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenProductNotFound()
        {
            var dto = new UpsertProductVariantDto { Id = 1, ProductId = 1, SizeId = 2, ColorId = 3 };
            var command = new UpdateProductVariantCommand(dto);
            var entity = new ProductVariantEntity { Id = dto.Id };

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(entity);
            _productRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenSizeNotFound()
        {
            var dto = new UpsertProductVariantDto { Id = 1, ProductId = 1, SizeId = 2, ColorId = 3 };
            var command = new UpdateProductVariantCommand(dto);
            var entity = new ProductVariantEntity { Id = dto.Id };

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(entity);
            _productRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenColorNotFound()
        {
            var dto = new UpsertProductVariantDto { Id = 1, ProductId = 1, SizeId = 2, ColorId = 3 };
            var command = new UpdateProductVariantCommand(dto);
            var entity = new ProductVariantEntity { Id = dto.Id };

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(entity);
            _productRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);
            _colorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Color, bool>>>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowInvalidOperationException_WhenVariantAlreadyExists()
        {
            var dto = new UpsertProductVariantDto { Id = 1, ProductId = 1, SizeId = 2, ColorId = 3 };
            var command = new UpdateProductVariantCommand(dto);
            var entity = new ProductVariantEntity { Id = dto.Id };

            _productVariantRepositoryMock.Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(entity);
            _productRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);
            _colorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Color, bool>>>(), It.IsAny<CancellationToken>()))
                                .ReturnsAsync(true);
            _productVariantRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
