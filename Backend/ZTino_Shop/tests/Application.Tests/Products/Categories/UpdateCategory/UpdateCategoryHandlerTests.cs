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
            var request = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNameAlreadyExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };
            var request = new UpdateCategoryCommand(dto);

            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentCategoryDoesNotExist()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 2 };
            var request = new UpdateCategoryCommand(dto);
            var existingCategory = new Category { Id = dto.Id, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .SetupSequence(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateCategory_WhenValid()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 2 };
            var request = new UpdateCategoryCommand(dto);

            var existingCategory = new Category { Id = dto.Id, Name = "OldName", ParentId = 1 };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingCategory);

            _categoryRepoMock
                .SetupSequence(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false)
                .ReturnsAsync(true);

            _mapperMock.Setup(m => m.Map(dto, existingCategory)).Verifiable();
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(existingCategory)).Returns(dto);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.Equal("Shirts", result.Name);

            _categoryRepoMock.Verify(r => r.Update(existingCategory), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
