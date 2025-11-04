using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Repositories;
using Application.Features.Products.Services.Commands.Categories.DeleteCategory;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.DeleteCategory
{
    public class DeleteCategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteCategoryService _service;

        public DeleteCategoryServiceTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _service = new DeleteCategoryService(
                _categoryRepoMock.Object,
                _contextMock.Object
                );
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrow_WhenIdInvalid()
        {
            int id = 1;

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveCategory_WhenIdValid()
        {
            int id = 1;
            var category = new Category { Id = id, Name = "Shirts" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            await _service.DeleteAsync(id);

            _categoryRepoMock.Verify(r => r.Remove(category), Times.Once);

            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
