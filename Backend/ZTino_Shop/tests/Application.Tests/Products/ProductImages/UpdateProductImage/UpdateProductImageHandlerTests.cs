using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.ProductImages.UpdateProductImage
{
    public class UpdateProductImageHandlerTests
    {
        private readonly Mock<IProductImageRepository> _productImageRepositoryMock;
        private readonly Mock<IProductColorRepository> _productColorRepositoryMock;
        private readonly Mock<IFileUploadService> _fileServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateProductImageHandler _handler;

        public UpdateProductImageHandlerTests()
        {
            _productImageRepositoryMock = new Mock<IProductImageRepository>();
            _productColorRepositoryMock = new Mock<IProductColorRepository>();
            _fileServiceMock = new Mock<IFileUploadService>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateProductImageHandler(
                _productImageRepositoryMock.Object,
                _productColorRepositoryMock.Object,
                _fileServiceMock.Object,
                _mapperMock.Object,
                _contextMock.Object);
        }

        [Fact]
        public async Task Handle_ImageNotFound_ThrowsNotFoundException()
        {
            _productImageRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductImage?)null);

            var command = new UpdateProductImageCommand(new UpsertProductImageDto { Id = 1 });

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ProductColorNotFound_ThrowsNotFoundException()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10 };
            _productImageRepositoryMock
                .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _productColorRepositoryMock
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductColor?)null);

            var command = new UpdateProductImageCommand(new UpsertProductImageDto { Id = 1 });

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ValidRequest_UploadsImageWhenProvided()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10, ImageUrl = "old.jpg" };
            var dto = new UpsertProductImageDto
            {
                Id = 1,
                ImgContent = new MemoryStream(new byte[] { 1 }),
                ImgFileName = "new.jpg"
            };

            _productImageRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _productColorRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductColor());
            _fileServiceMock.Setup(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync("new-url.jpg");

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.Equal("new-url.jpg", entity.ImageUrl);
            _fileServiceMock.Verify(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_SetMainTrue_UnsetsCurrentMain()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10, IsMain = false };
            var currentMain = new ProductImage { Id = 2, ProductColorId = 10, IsMain = true };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = true };

            _productImageRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _productColorRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductColor());

            _productImageRepositoryMock
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(currentMain);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.True(entity.IsMain);
            Assert.False(currentMain.IsMain);
            _productImageRepositoryMock.Verify(x => x.Update(currentMain), Times.Once);
            _productImageRepositoryMock.Verify(x => x.Update(entity), Times.Once);
        }

        [Fact]
        public async Task Handle_SetMainFalse_AssignsAlternativeMain()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10, IsMain = true };
            var alternative = new ProductImage { Id = 2, ProductColorId = 10, IsMain = false };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = false };

            _productImageRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _productColorRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductColor());

            _productImageRepositoryMock
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync(alternative);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.False(entity.IsMain);
            Assert.True(alternative.IsMain);
            _productImageRepositoryMock.Verify(x => x.Update(alternative), Times.Once);
        }

        [Fact]
        public async Task Handle_SetMainFalse_StaysMainIfNoAlternative()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10, IsMain = true };
            var dto = new UpsertProductImageDto { Id = 1, IsMain = false };

            _productImageRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _productColorRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductColor());

            _productImageRepositoryMock
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), true, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductImage?)null);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.True(entity.IsMain);
        }

        [Fact]
        public async Task Handle_UpdateDisplayOrder_UpdatesSuccessfully()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10, DisplayOrder = 1 };
            var dto = new UpsertProductImageDto { Id = 1, DisplayOrder = 5 };

            _productImageRepositoryMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
            _productColorRepositoryMock.Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ProductColor());

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.Equal(5, entity.DisplayOrder);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}