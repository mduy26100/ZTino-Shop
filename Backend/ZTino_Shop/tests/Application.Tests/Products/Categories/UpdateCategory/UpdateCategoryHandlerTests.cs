using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Commands.Categories.UpdateCategory;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.UpdateCategory
{
    public class UpdateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateCategoryHandler _handler;

        public UpdateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateCategoryHandler(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotFound()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNameAlreadyExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };
            var command = new UpdateCategoryCommand(dto);
            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentDoesNotExist()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 99 };
            var command = new UpdateCategoryCommand(dto);
            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentIsNotRoot()
        {
            var dto = new UpsertCategoryDto { Id = 3, Name = "Shoes", ParentId = 2 };
            var command = new UpdateCategoryCommand(dto);

            var existingCategory = new Category { Id = 3, Name = "OldShoes" };
            var parent = new Category { Id = 2, Name = "ChildCategory", ParentId = 1 };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parent);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateSuccessfully_WhenValid()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "UpdatedName", ParentId = null };
            var command = new UpdateCategoryCommand(dto);

            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map(dto, existingCategory)).Verifiable();
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(existingCategory)).Returns(dto);

            _categoryRepoMock.Setup(r => r.Update(existingCategory)).Verifiable();
            _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("UpdatedName", result.Name);
            _mapperMock.Verify(m => m.Map(dto, existingCategory), Times.Once);
            _categoryRepoMock.Verify(r => r.Update(existingCategory), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
