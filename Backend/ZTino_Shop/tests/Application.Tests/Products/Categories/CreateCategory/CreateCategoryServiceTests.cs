using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.DTOs.Categories;
using Application.Features.Products.Repositories;
using Application.Features.Products.Services.Commands.Categories.CreateCategory;
using Domain.Models.Products;

namespace Application.Tests.Products.Categories.CreateCategory
{
    public class CreateCategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly CreateCategoryService _service;

        public CreateCategoryServiceTests()
        {
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();
            _service = new CreateCategoryService(
                _categoryRepoMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenParentNotExist()
        {
            var dto = new UpsertCategoryDto { Name = "Shirts", ParentId = 1 };

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenNameExists()
        {
            var dto = new UpsertCategoryDto { Name = "Shirts", ParentId = null };

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnDto_WhenSuccess()
        {
            var dto = new UpsertCategoryDto { Name = "Pants", ParentId = null };
            var entity = new Category { Name = "Pants" };

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map<Category>(dto)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            var result = await _service.CreateAsync(dto, CancellationToken.None);

            Assert.Equal("Pants", result.Name);
            _categoryRepoMock.Verify(r => r.AddAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}