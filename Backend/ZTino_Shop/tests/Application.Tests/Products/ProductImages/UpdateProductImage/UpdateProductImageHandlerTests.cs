using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.Repositories;
using Application.Common.Models;
using Domain.Models.Products;

namespace Application.Tests.Features.Products.Commands.ProductImages.UpdateProductImage
{
    public class UpdateProductImageHandlerTests
    {
        private readonly Mock<IProductImageRepository> _repoMock;
        private readonly Mock<IFileUploadService> _fileServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateProductImageHandler _handler;

        public UpdateProductImageHandlerTests()
        {
            _repoMock = new Mock<IProductImageRepository>();
            _fileServiceMock = new Mock<IFileUploadService>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateProductImageHandler(
                _repoMock.Object,
                _fileServiceMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldThrow_WhenImageNotFound()
        {
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductImage?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(new UpdateProductImageCommand(new UpsertProductImageDto { Id = 999 }), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUploadImage_WhenContentProvided()
        {
            var entity = new ProductImage { Id = 1, IsMain = false };
            var dto = new UpsertProductImageDto
            {
                Id = 1,
                IsMain = false,
                ImgContent = new MemoryStream(new byte[] { 1 }),
                ImgFileName = "test.jpg"
            };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<UpsertProductImageDto>(It.IsAny<ProductImage>())).Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            _fileServiceMock.Verify(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()), Times.Once);
            _repoMock.Verify(x => x.Update(entity), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ShouldRouteTo_ClaimMainStrategy_WhenChangingFalseToTrue()
        {
            var entity = new ProductImage { Id = 1, IsMain = false, ProductVariantId = 10 };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = true };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<UpsertProductImageDto>(It.IsAny<ProductImage>())).Returns(dto);

            _repoMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            _repoMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRouteTo_ResignMainStrategy_WhenChangingTrueToFalse()
        {
            var entity = new ProductImage { Id = 1, IsMain = true, ProductVariantId = 10 };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = false };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<UpsertProductImageDto>(It.IsAny<ProductImage>())).Returns(dto);

            _repoMock.Setup(x => x.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            _repoMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldRouteTo_DefaultStrategy_WhenNoChangeInIsMain()
        {
            var entity = new ProductImage { Id = 1, IsMain = true };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = true, DisplayOrder = 99 };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _mapperMock.Setup(m => m.Map<UpsertProductImageDto>(It.IsAny<ProductImage>())).Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            _repoMock.Verify(x => x.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Never);

            Assert.Equal(99, entity.DisplayOrder);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}