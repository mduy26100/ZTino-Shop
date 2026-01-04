using Application.Features.Products.v1.Commands.Products.UpdateProduct;
using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Repositories;
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
            var dto = new UpsertProductDto { Id = 1 };
            var command = new UpdateProductCommand(dto);

            _productRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotFound()
        {
            var dto = new UpsertProductDto { Id = 1, CategoryId = 99 };
            var command = new UpdateProductCommand(dto);

            var existingProduct = new Product { Id = 1 };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryIsRoot()
        {
            var dto = new UpsertProductDto { Id = 1, CategoryId = 2 };
            var command = new UpdateProductCommand(dto);

            var existingProduct = new Product { Id = 1 };
            var rootCategory = new Category { Id = 2, ParentId = null };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rootCategory);

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProductNameAlreadyExists()
        {
            var dto = new UpsertProductDto
            {
                Id = 1,
                CategoryId = 2,
                Name = "Duplicate"
            };
            var command = new UpdateProductCommand(dto);

            var existingProduct = new Product { Id = 1, Name = "Old" };
            var subCategory = new Category { Id = 2, ParentId = 10 };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subCategory);

            _productRepoMock
                .Setup(r => r.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenValid()
        {
            var dto = new UpsertProductDto
            {
                Id = 1,
                CategoryId = 2,
                Name = "Updated Product",
                ImgContent = null,
                ImgFileName = null
            };
            var command = new UpdateProductCommand(dto);

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Old",
                MainImageUrl = "old.jpg"
            };

            var subCategory = new Category
            {
                Id = 2,
                ParentId = 10
            };

            var expectedDto = new UpsertProductDto
            {
                Id = 1,
                Name = "Updated Product"
            };

            _productRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingProduct);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subCategory);

            _productRepoMock
                .Setup(r => r.AnyAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map(dto, existingProduct))
                .Callback(() =>
                {
                    existingProduct.Name = dto.Name;
                });

            _mapperMock
                .Setup(m => m.Map<UpsertProductDto>(existingProduct))
                .Returns(expectedDto);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Name, result.Name);

            _productRepoMock.Verify(r => r.Update(existingProduct), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}