using Application.Features.Products.v1.Commands.Categories.UpdateCategory;
using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.Categories.UpdateCategory
{
    public class UpdateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFileUploadService> _fileUploadMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateCategoryHandler _handler;

        public UpdateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _fileUploadMock = new Mock<IFileUploadService>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateCategoryHandler(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _fileUploadMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotFound()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", Slug = "shirts" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenDuplicateNameExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", Slug = "shirts-new" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2, Name = "Shirts" });

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenDuplicateSlugExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", Slug = "shirts" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2, Name = "Shirts", Slug = "shirts" });

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentDoesNotExist()
        {
            var dto = new UpsertCategoryDto
            {
                Id = 1,
                Name = "Shirts",
                Slug = "shirts",
                ParentId = 99
            };

            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentIsNotRoot()
        {
            var dto = new UpsertCategoryDto
            {
                Id = 3,
                Name = "Shoes",
                Slug = "shoes",
                ParentId = 2
            };

            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 3 });

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2, ParentId = 1 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenChildCategoryUploadsImage()
        {
            var dto = new UpsertCategoryDto
            {
                Id = 1,
                Name = "Child",
                Slug = "child",
                ParentId = 2,
                ImgContent = new MemoryStream(new byte[] { 1, 2, 3 }),
                ImgFileName = "image.jpg"
            };

            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId!.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateAndUploadImage_WhenRootCategory()
        {
            var dto = new UpsertCategoryDto
            {
                Id = 1,
                Name = "Root",
                Slug = "root",
                ImgContent = new MemoryStream(new byte[] { 1 }),
                ImgFileName = "root.jpg"
            };

            var command = new UpdateCategoryCommand(dto);
            var entity = new Category { Id = 1, Name = "Old", Slug = "old" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _categoryRepoMock
                .Setup(r => r.FindOneAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            _fileUploadMock
                .Setup(f => f.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("https://cdn.com/root.jpg");

            _mapperMock.Setup(m => m.Map(dto, entity));
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Root", result.Name);
            _categoryRepoMock.Verify(r => r.Update(entity), Times.Once);
            _fileUploadMock.Verify(f =>
                f.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
