using Application.Common.Interfaces.Persistence.EFCore;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models;
using Application.Features.Products.Commands.Products.UpdateProduct;
using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Products.UpdateProduct
{
    public class UpdateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IFileUploadService> _fileUploadMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateProductHandler _handler;

        public UpdateProductHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _fileUploadMock = new Mock<IFileUploadService>();
            _contextMock = new Mock<IApplicationDbContext>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateProductHandler(
                _productRepoMock.Object,
                _categoryRepoMock.Object,
                _fileUploadMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProductNotFound()
        {
            var command = new UpdateProductCommand(new UpsertProductDto { Id = 1 });

            _productRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotFound()
        {
            var dto = new UpsertProductDto { Id = 1, CategoryId = 99 };
            var command = new UpdateProductCommand(dto);
            var existingProduct = new Product { Id = 1, Name = "Old" };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(c => c.Id == 99, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNameAlreadyExists()
        {
            var dto = new UpsertProductDto { Id = 1, CategoryId = 1, Name = "Duplicate" };
            var command = new UpdateProductCommand(dto);
            var existingProduct = new Product { Id = 1, Name = "Old" };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenValid()
        {
            var dto = new UpsertProductDto
            {
                Id = 1,
                CategoryId = 2,
                Name = "Updated Product",
            };
            var command = new UpdateProductCommand(dto);

            var existingProduct = new Product { Id = 1, Name = "Old", MainImageUrl = "old.jpg" };
            var updatedProduct = new Product { Id = 1, Name = "Updated Product", MainImageUrl = "new.jpg" };
            var expectedDto = new UpsertProductDto { Id = 1, Name = "Updated Product" };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _fileUploadMock
                .Setup(f => f.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("https://example.com/new.jpg");

            _mapperMock
                .Setup(m => m.Map(dto, existingProduct))
                .Callback(() =>
                {
                    existingProduct.Name = dto.Name;
                    existingProduct.MainImageUrl = "https://example.com/new.jpg";
                });

            _mapperMock
                .Setup(m => m.Map<UpsertProductDto>(existingProduct))
                .Returns(expectedDto);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Name, result.Name);
            _productRepoMock.Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}