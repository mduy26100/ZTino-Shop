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
                _contextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldThrowNotFound_WhenImageDoesNotExist()
        {
            _repoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductImage?)null);

            var command = new UpdateProductImageCommand(
                new UpsertProductImageDto { Id = 999 });

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldUploadImage_WhenImageContentProvided()
        {
            var entity = new ProductImage { Id = 1, ProductColorId = 10 };
            var dto = new UpsertProductImageDto
            {
                Id = 1,
                ImgContent = new MemoryStream(new byte[] { 1, 2, 3 }),
                ImgFileName = "test.jpg"
            };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _repoMock.Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            _fileServiceMock.Setup(x => x.UploadAsync(
                    It.IsAny<FileUploadRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync("https://cdn.test/image.jpg");

            _mapperMock.Setup(x => x.Map<UpsertProductImageDto>(It.IsAny<ProductImage>()))
                .Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            _fileServiceMock.Verify(x =>
                x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()),
                Times.Once);

            _contextMock.Verify(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldClaimMain_WhenRequestedIsMain()
        {
            var current = new ProductImage
            {
                Id = 1,
                ProductColorId = 10,
                IsMain = false
            };

            var oldMain = new ProductImage
            {
                Id = 2,
                ProductColorId = 10,
                IsMain = true
            };

            var dto = new UpsertProductImageDto
            {
                Id = 1,
                IsMain = true
            };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(current);

            _repoMock.Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage> { oldMain });

            _mapperMock.Setup(x => x.Map<UpsertProductImageDto>(It.IsAny<ProductImage>()))
                .Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.True(current.IsMain);
            Assert.False(oldMain.IsMain);

            _repoMock.Verify(x => x.Update(oldMain), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldResignMain_AndAssignHeir()
        {
            var current = new ProductImage
            {
                Id = 1,
                ProductColorId = 10,
                IsMain = true
            };

            var heir = new ProductImage
            {
                Id = 2,
                ProductColorId = 10,
                IsMain = false,
                DisplayOrder = 1
            };

            var dto = new UpsertProductImageDto
            {
                Id = 1,
                IsMain = false
            };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(current);

            _repoMock.Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage> { heir });

            _mapperMock.Setup(x => x.Map<UpsertProductImageDto>(It.IsAny<ProductImage>()))
                .Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.False(current.IsMain);
            Assert.True(heir.IsMain);

            _repoMock.Verify(x => x.Update(heir), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateDisplayOrder_WhenProvided()
        {
            var entity = new ProductImage { Id = 1, DisplayOrder = 1, ProductColorId = 10 };

            var dto = new UpsertProductImageDto
            {
                Id = 1,
                DisplayOrder = 99
            };

            _repoMock.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _repoMock.Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<ProductImage, bool>>>(),
                    false,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            _mapperMock.Setup(x => x.Map<UpsertProductImageDto>(It.IsAny<ProductImage>()))
                .Returns(dto);

            await _handler.Handle(new UpdateProductImageCommand(dto), CancellationToken.None);

            Assert.Equal(99, entity.DisplayOrder);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}