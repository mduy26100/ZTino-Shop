using Application.Features.Products.v1.Commands.Products.CreateProduct;
using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.Products.CreateProduct
{
    public class CreateProductHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IFileUploadService> _fileUploadServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly CreateProductHandler _handler;

        public CreateProductHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _fileUploadServiceMock = new Mock<IFileUploadService>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new CreateProductHandler(
                _productRepoMock.Object,
                _categoryRepoMock.Object,
                _fileUploadServiceMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotExists()
        {
            var dto = new UpsertProductDto { CategoryId = 1, Name = "T-Shirt" };
            var command = new CreateProductCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryIsRoot()
        {
            var dto = new UpsertProductDto { CategoryId = 1, Name = "T-Shirt" };
            var command = new CreateProductCommand(dto);

            var rootCategory = new Category
            {
                Id = 1,
                ParentId = null
            };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(rootCategory);

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenProductNameExists()
        {
            var dto = new UpsertProductDto { CategoryId = 1, Name = "T-Shirt" };
            var command = new CreateProductCommand(dto);

            var subCategory = new Category
            {
                Id = 1,
                ParentId = 10
            };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subCategory);

            _productRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnDto_WhenSuccess()
        {
            var dto = new UpsertProductDto
            {
                CategoryId = 1,
                Name = "T-Shirt",
                ImgFileName = null,
                ImgContent = null
            };
            var command = new CreateProductCommand(dto);

            var subCategory = new Category
            {
                Id = 1,
                ParentId = 10
            };

            var entity = new Product
            {
                Id = 10,
                Name = "T-Shirt",
                CategoryId = 1,
                MainImageUrl = "image.jpg"
            };

            var expectedDto = new UpsertProductDto
            {
                Id = 10,
                Name = "T-Shirt",
                CategoryId = 1,
                MainImageUrl = "image.jpg"
            };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.CategoryId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(subCategory);

            _productRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Product>(It.IsAny<UpsertProductDto>()))
                .Returns(entity);

            _mapperMock
                .Setup(m => m.Map<UpsertProductDto>(It.IsAny<Product>()))
                .Returns(expectedDto);

            _productRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.Name, result.Name);
            Assert.Equal(expectedDto.CategoryId, result.CategoryId);
            Assert.Equal(expectedDto.MainImageUrl, result.MainImageUrl);

            _productRepoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}