using Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant;
using Application.Features.Products.v1.DTOs.ProductVariants;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.ProductVariant.CreateProductVariant
{
    public class CreateProductVariantHandlerTests
    {
        private readonly Mock<IProductVariantRepository> _productVariantRepositoryMock;
        private readonly Mock<IProductColorRepository> _productColorRepositoryMock;
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateProductVariantHandler _handler;

        public CreateProductVariantHandlerTests()
        {
            _productVariantRepositoryMock = new Mock<IProductVariantRepository>();
            _productColorRepositoryMock = new Mock<IProductColorRepository>();
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateProductVariantHandler(
                _productVariantRepositoryMock.Object,
                _productColorRepositoryMock.Object,
                _sizeRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_Should_CreateProductVariant_WhenAllValid()
        {
            var dto = new UpsertProductVariantDto
            {
                ProductColorId = 10,
                SizeId = 2,
                StockQuantity = 10,
                Price = 100,
                IsActive = true
            };
            var command = new CreateProductVariantCommand(dto);

            var entity = new ProductVariantEntity
            {
                Id = 1,
                ProductColorId = dto.ProductColorId,
                SizeId = dto.SizeId,
                StockQuantity = dto.StockQuantity,
                Price = dto.Price,
                IsActive = dto.IsActive
            };

            _productColorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(true);

            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

            _productVariantRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<ProductVariantEntity>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UpsertProductVariantDto>(entity)).Returns(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            _productVariantRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductVariantEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenProductColorNotFound()
        {
            var dto = new UpsertProductVariantDto { ProductColorId = 1, SizeId = 2 };
            var command = new CreateProductVariantCommand(dto);

            _productColorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _productVariantRepositoryMock.Verify(r => r.AddAsync(It.IsAny<ProductVariantEntity>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_WhenSizeNotFound()
        {
            var dto = new UpsertProductVariantDto { ProductColorId = 1, SizeId = 2 };
            var command = new CreateProductVariantCommand(dto);

            _productColorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(false);

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_ThrowConflictException_WhenVariantAlreadyExists()
        {
            var dto = new UpsertProductVariantDto { ProductColorId = 1, SizeId = 2 };
            var command = new CreateProductVariantCommand(dto);

            _productColorRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<CancellationToken>()))
                                       .ReturnsAsync(true);
            _sizeRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(true);

            _productVariantRepositoryMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<CancellationToken>()))
                                         .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}