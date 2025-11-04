using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;
using Application.Features.Products.Services.Commands.Categories.UpdateCategory;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.UpdateCategory
{
    public class UpdateCategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateCategoryService _service;

        public UpdateCategoryServiceTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();
            _service = new UpdateCategoryService(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
                );
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenIdInvalid()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenNameExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts"};

            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };
            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrow_WhenParentNotExist()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 2 };

            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };
            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _service.UpdateAsync(dto));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnDto_WhenSuccess()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 2 };
            var existingCategory = new Category { Id = dto.Id, Name = "OldName", ParentId = 1 };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .SetupSequence(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(existingCategory);
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(existingCategory)).Returns(dto);

            var result = await _service.UpdateAsync(dto, CancellationToken.None);

            Assert.Equal("Shirts", result.Name);

            _categoryRepoMock.Verify(r => r.Update(existingCategory), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
