using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Commands.Categories.DeleteCategory;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.DeleteCategory
{
    public class DeleteCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DeleteCategoryHandler _handler;

        public DeleteCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new DeleteCategoryHandler(
                _categoryRepoMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenCategoryNotFound()
        {
            int id = 1;
            var command = new DeleteCategoryCommand(id);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _handler.Handle(command, CancellationToken.None)
            );
        }

        [Fact]
        public async Task Handle_ShouldRemoveCategory_WhenCategoryExists()
        {
            int id = 1;
            var category = new Category { Id = id, Name = "Shirts" };
            var command = new DeleteCategoryCommand(id);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            await _handler.Handle(command, CancellationToken.None);

            _categoryRepoMock.Verify(r => r.Remove(category), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
