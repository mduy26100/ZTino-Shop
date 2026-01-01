using System.Linq.Expressions;
using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Features.Products.v1.Commands.Categories.UpdateCategory;
using Application.Features.Products.v1.DTOs.Categories;
using Application.Features.Products.v1.Repositories;
using AutoMapper;
using Domain.Models.Products;
using Moq;
using Xunit;

namespace Application.Tests.Products.Categories.UpdateCategory
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
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenNameAlreadyExists()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts" };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentDoesNotExist()
        {
            var dto = new UpsertCategoryDto { Id = 1, Name = "Shirts", ParentId = 99 };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenParentIsNotRoot()
        {
            var dto = new UpsertCategoryDto { Id = 3, Name = "Shoes", ParentId = 2 };
            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 3 });

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

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
                ParentId = 2,
                ImgContent = new MemoryStream(new byte[] { 1, 2, 3 }),
                ImgFileName = "image.jpg"
            };

            var command = new UpdateCategoryCommand(dto);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 1 });

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.ParentId!.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Category { Id = 2 });

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUpdateSuccessfully_WhenValid()
        {
            var dto = new UpsertCategoryDto
            {
                Id = 1,
                Name = "UpdatedName"
            };

            var command = new UpdateCategoryCommand(dto);
            var entity = new Category { Id = 1, Name = "OldName" };

            _categoryRepoMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _categoryRepoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(m => m.Map(dto, entity));
            _mapperMock.Setup(m => m.Map<UpsertCategoryDto>(entity)).Returns(dto);

            _contextMock
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal("UpdatedName", result.Name);
            _categoryRepoMock.Verify(r => r.Update(entity), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
