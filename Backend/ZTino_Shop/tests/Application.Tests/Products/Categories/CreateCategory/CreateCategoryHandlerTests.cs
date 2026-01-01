using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.Commands.Categories.CreateCategory;
using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.CreateCategory
{
    public class CreateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileUploadService> _fileUploadMock;
        private readonly Mock<IApplicationDbContext> _contextMock;

        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _fileUploadMock = new Mock<IFileUploadService>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new CreateCategoryHandler(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _fileUploadMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentNotExist()
        {
            var dto = new UpsertCategoryDto { Name = "Child", ParentId = 1 };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentIsNotRoot()
        {
            var dto = new UpsertCategoryDto { Name = "Child", ParentId = 2 };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2, ParentId = 1 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenChildHasImageUrl()
        {
            var dto = new UpsertCategoryDto
            {
                Name = "Child",
                ParentId = 1,
                ImageUrl = "https://img.com/a.jpg"
            };

            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenChildHasImageFile()
        {
            var dto = new UpsertCategoryDto
            {
                Name = "Child",
                ParentId = 1,
                ImgContent = new MemoryStream(new byte[] { 1, 2 }),
                ImgFileName = "img.jpg"
            };

            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenDuplicateName()
        {
            var dto = new UpsertCategoryDto { Name = "Men", Slug = "men" };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Name = "Men" });

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUploadImage_WhenRootCategoryHasImageFile()
        {
            var dto = new UpsertCategoryDto
            {
                Name = "Men",
                ImgContent = new MemoryStream(new byte[] { 1, 2, 3 }),
                ImgFileName = "men.jpg"
            };

            var command = new CreateCategoryCommand(dto);
            var entity = new Category { Name = "Men" };

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _fileUploadMock
                .Setup(f => f.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("https://cdn.com/men.jpg");

            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            _categoryRepoMock
                .Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Men", result.Name);
            _fileUploadMock.Verify(
                f => f.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldCreateSuccessfully_WithoutImage()
        {
            var dto = new UpsertCategoryDto { Name = "Pants" };
            var command = new CreateCategoryCommand(dto);
            var entity = new Category { Name = "Pants" };

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            _categoryRepoMock
                .Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Pants", result.Name);
            _categoryRepoMock.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
