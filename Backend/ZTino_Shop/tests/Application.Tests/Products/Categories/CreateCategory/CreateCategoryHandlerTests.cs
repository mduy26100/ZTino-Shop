using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Commands.Categories.CreateCategory;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.CreateCategory
{
    public class CreateCategoryHandlerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly CreateCategoryHandler _handler;

        public CreateCategoryHandlerTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new CreateCategoryHandler(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentNotExist()
        {
            var dto = new UpsertCategoryDto { Name = "Shirts", ParentId = 1 };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentIsNotRoot()
        {
            var dto = new UpsertCategoryDto { Name = "ChildCategory", ParentId = 2 };
            var command = new CreateCategoryCommand(dto);

            var parent = new Category { Id = 2, Name = "ParentCategory", ParentId = 1 };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(2, It.IsAny<CancellationToken>()))
                .ReturnsAsync(parent);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNameExists()
        {
            var dto = new UpsertCategoryDto { Name = "Shirts" };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldReturnDto_WhenSuccess()
        {
            var dto = new UpsertCategoryDto { Name = "Pants", ParentId = null };
            var entity = new Category { Name = "Pants" };
            var command = new CreateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            _categoryRepoMock.Setup(r => r.AddAsync(entity, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(1));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("Pants", result.Name);
            _categoryRepoMock.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
