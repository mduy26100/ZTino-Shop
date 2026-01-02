using Application.Common.Interfaces.Persistence.Data;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models.Requests;
using Application.Features.Products.v1.Commands.ProductImages.CreateProductImages;
using Application.Features.Products.v1.DTOs.ProductImages;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.ProductImages.CreateProductImages
{
    public class CreateProductImagesHandlerTests
    {
        private readonly Mock<IProductImageRepository> _productImageRepository = new();
        private readonly Mock<IProductVariantRepository> _productVariantRepository = new();
        private readonly Mock<IFileUploadService> _fileUploadService = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IApplicationDbContext> _context = new();

        private readonly CreateProductImagesHandler _handler;

        public CreateProductImagesHandlerTests()
        {
            _handler = new CreateProductImagesHandler(
                _productImageRepository.Object,
                _productVariantRepository.Object,
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
        public async Task Handle_VariantNotExists_ThrowsNotFoundException()
        {
            _productVariantRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductVariantEntity?)null);

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductVariantId = 1 }
            });

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NoExistingImages_FirstImageIsMain()
        {
            const int variantId = 1;
            const int productColorId = 10;
            var variant = new ProductVariantEntity { Id = variantId, ProductColorId = productColorId };

            _productVariantRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

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
                    ProductVariantId = variantId,
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
            const int variantId = 1;
            const int productColorId = 10;
            var variant = new ProductVariantEntity { Id = variantId, ProductColorId = productColorId };

            _productVariantRepository
                .Setup(x => x.FindOneAsync(It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(variant);

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
                new()
                {
                    ProductVariantId = variantId,
                    ImgContent = new MemoryStream(new byte[] { 1 }),
                    ImgFileName = "a.jpg"
                },
                new()
                {
                    ProductVariantId = variantId,
                    ImgContent = new MemoryStream(new byte[] { 2 }),
                    ImgFileName = "b.jpg"
                }
            });

            await _handler.Handle(command, CancellationToken.None);

            _productImageRepository.Verify(x =>
                x.AddRangeAsync(
                    It.Is<List<ProductImage>>(list =>
                        list.All(i => i.ProductColorId == productColorId) &&
                        list.All(i => !i.IsMain) &&
                        list.Select(i => i.DisplayOrder).SequenceEqual(new[] { 4, 5 })),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}