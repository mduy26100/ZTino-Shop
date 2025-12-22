using Application.Common.Interfaces.Persistence.EFCore;
using Application.Common.Interfaces.Services.FileUpLoad;
using Application.Common.Models;
using Application.Features.Products.Commands.ProductImages.CreateProductImage;
using Application.Features.Products.DTOs.ProductImages;
using Application.Features.Products.Repositories;
using Domain.Models.Products;
using ProductVariantEntity = Domain.Models.Products.ProductVariant;

namespace Application.Tests.Products.ProductImages.CreateProductImages
{
    public class CreateProductImagesHandlerTests
    {
        private readonly Mock<IProductImageRepository> _productImageRepository;
        private readonly Mock<IProductVariantRepository> _productVariantRepository;
        private readonly Mock<IFileUploadService> _fileUploadService;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IApplicationDbContext> _context;

        private readonly CreateProductImagesHandler _handler;

        public CreateProductImagesHandlerTests()
        {
            _productImageRepository = new Mock<IProductImageRepository>();
            _productVariantRepository = new Mock<IProductVariantRepository>();
            _fileUploadService = new Mock<IFileUploadService>();
            _mapper = new Mock<IMapper>();
            _context = new Mock<IApplicationDbContext>();

            _handler = new CreateProductImagesHandler(
                _productImageRepository.Object,
                _productVariantRepository.Object,
                _fileUploadService.Object,
                _mapper.Object,
                _context.Object);
        }

        [Fact]
        public async Task Handle_DtosEmpty_ReturnsEmptyList()
        {
            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>());

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_DifferentVariantIds_ThrowsArgumentException()
        {
            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductVariantId = 1 },
                new() { ProductVariantId = 2 }
            });

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_VariantNotExists_ThrowsKeyNotFoundException()
        {
            _productVariantRepository
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductVariantId = 1 }
            });

            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_NoExistingImages_FirstImageIsMain()
        {
            const int variantId = 1;

            _productVariantRepository
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productImageRepository
                .Setup(x => x.GetMaxDisplayOrderAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _fileUploadService
                .Setup(x => x.UploadAsync(It.IsAny<FileUploadRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync("http://img.test/1.jpg");

            _mapper
                .Setup(x => x.Map<ProductImage>(It.IsAny<UpsertProductImageDto>()))
                .Returns(new ProductImage { ProductVariantId = variantId });

            _mapper
                .Setup(x => x.Map<List<UpsertProductImageDto>>(It.IsAny<List<ProductImage>>()))
                .Returns(new List<UpsertProductImageDto>());

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new()
                {
                    ProductVariantId = variantId,
                    ImgContent = new MemoryStream(new byte[] { 1, 2, 3 }),
                    ImgFileName = "img.jpg",
                    ImgContentType = "image/jpeg"
                }
            });

            await _handler.Handle(command, CancellationToken.None);

            _productImageRepository.Verify(x =>
                x.AddRangeAsync(
                    It.Is<List<ProductImage>>(list =>
                        list.Count == 1 &&
                        list[0].IsMain == true &&
                        list[0].DisplayOrder == 1),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_HasExistingImages_NewImagesAreNotMain()
        {
            const int variantId = 1;

            _productVariantRepository
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productImageRepository
                .Setup(x => x.GetMaxDisplayOrderAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(3);

            _mapper
                .Setup(x => x.Map<ProductImage>(It.IsAny<UpsertProductImageDto>()))
                .Returns(new ProductImage { ProductVariantId = variantId });

            _mapper
                .Setup(x => x.Map<List<UpsertProductImageDto>>(It.IsAny<List<ProductImage>>()))
                .Returns(new List<UpsertProductImageDto>());

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductVariantId = variantId }
            });

            await _handler.Handle(command, CancellationToken.None);

            _productImageRepository.Verify(x =>
                x.AddRangeAsync(
                    It.Is<List<ProductImage>>(list =>
                        list.All(i => i.IsMain == false)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ValidRequest_SaveChangesCalled()
        {
            const int variantId = 1;

            _productVariantRepository
                .Setup(x => x.AnyAsync(
                    It.IsAny<Expression<Func<ProductVariantEntity, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _productImageRepository
                .Setup(x => x.GetMaxDisplayOrderAsync(variantId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            _mapper
                .Setup(x => x.Map<ProductImage>(It.IsAny<UpsertProductImageDto>()))
                .Returns(new ProductImage { ProductVariantId = variantId });

            _mapper
                .Setup(x => x.Map<List<UpsertProductImageDto>>(It.IsAny<List<ProductImage>>()))
                .Returns(new List<UpsertProductImageDto>());

            var command = new CreateProductImagesCommand(new List<UpsertProductImageDto>
            {
                new() { ProductVariantId = variantId }
            });

            await _handler.Handle(command, CancellationToken.None);

            _context.Verify(
                x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}