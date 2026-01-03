using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.Commands.ProductImages.CreateProductImages;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.ProductImages.CreateProductImages
{
    public class CreateProductImagesHandlerTests
    {
        private readonly Mock<IProductImageRepository> _productImageRepository = new();
        private readonly Mock<IProductColorRepository> _productColorRepository = new();
        private readonly Mock<IFileUploadService> _fileUploadService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private readonly CreateProductImagesHandler _handler;

        public CreateProductImagesHandlerTests()
        {
            _handler = new CreateProductImagesHandler(
                _productImageRepository.Object,
                _productColorRepository.Object,
                _fileUploadService.Object,
                _mapper.Object,
                _context.Object);
        }

        [Fact]
        public async Task Handle_DtosNullOrEmpty_ReturnsEmptyList()
        {
            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_ProductColorNotExists_ThrowsNotFoundException()
        {
            _productColorRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductColor?)null);

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductColorId = 1 }
            });

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NoExistingImages_FirstImageIsMain()
        {
            const int productColorId = 1;
            var productColor = new ProductColor { Id = productColorId };

            _productColorRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productColor);

            _productImageRepository
                .Setup(x => x.GetMaxDisplayOrderAsync(productColorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _fileUploadService
                .Setup(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("http://img.test/1.jpg");

            _mapper
                .Setup(x => x.Map<ProductImage>(It.IsAny<UpsertProductImageDto>()))
                .Returns(new ProductImage());

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new()
                {
                    ProductColorId = productColorId,
                    ImgContent = new MemoryStream(new byte[] { 1, 2 }),
                    ImgFileName = "img.jpg"
                }
            });

            await _handler.Handle(command, CancellationToken.None);

            _productImageRepository.Verify(x =>
                x.AddRangeAsync(
                    It.Is<List<ProductImage>>(list =>
                        list.Count == 1 &&
                        list[0].ProductColorId == productColorId &&
                        list[0].IsMain &&
                        list[0].DisplayOrder == 1),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_HasExistingImages_AllNewImagesAreNotMain()
        {
            const int productColorId = 1;
            var productColor = new ProductColor { Id = productColorId };

            _productColorRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductColor, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productColor);

            _productImageRepository
                .Setup(x => x.GetMaxDisplayOrderAsync(productColorId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(3);

            _fileUploadService
                .Setup(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("http://img.test/x.jpg");

            _mapper
                .Setup(x => x.Map<ProductImage>(It.IsAny<UpsertProductImageDto>()))
                .Returns(() => new ProductImage());

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductColorId = productColorId, ImgContent = new MemoryStream(new byte[]{1}), ImgFileName = "a.jpg" },
                new() { ProductColorId = productColorId, ImgContent = new MemoryStream(new byte[]{2}), ImgFileName = "b.jpg" }
            });

            await _handler.Handle(command, CancellationToken.None);

            _productImageRepository.Verify(x =>
                x.AddRangeAsync(
                    It.Is<List<ProductImage>>(list =>
                        list.Count == 2 &&
                        list.All(i => i.ProductColorId == productColorId) &&
                        list.All(i => !i.IsMain) &&
                        list[0].DisplayOrder == 4 &&
                        list[1].DisplayOrder == 5),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}