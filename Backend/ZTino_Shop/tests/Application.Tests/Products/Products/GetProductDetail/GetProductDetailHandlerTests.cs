using Application.Features.Products.DTOs.Products;
using Application.Features.Products.Queries.Products.GetProductDetail;
using Application.Features.Products.Services;

namespace Application.Tests.Products.Products.GetProductDetail
{
    public class GetProductDetailHandlerTests
    {
        private readonly Mock<IProductQueryService> _productQueryServiceMock;
        private readonly GetProductDetailHandler _handler;

        public GetProductDetailHandlerTests()
        {
            _productQueryServiceMock = new Mock<IProductQueryService>();
            _handler = new GetProductDetailHandler(_productQueryServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ProductExists_ReturnsProductDetailDto()
        {
            var productId = 1;
            var query = new GetProductDetailQuery(productId);

            var productDto = new ProductDetailDto
            {
                Id = productId,
                Name = "Test Product",
                Slug = "test-product",
                BasePrice = 100_000
            };

            _productQueryServiceMock
                .Setup(x => x.GetProductDetailAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(productDto);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(productId, result!.Id);
            Assert.Equal("Test Product", result.Name);

            _productQueryServiceMock.Verify(
                x => x.GetProductDetailAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsNotFoundException()
        {
            var productId = 999;
            var query = new GetProductDetailQuery(productId);

            _productQueryServiceMock
                .Setup(x => x.GetProductDetailAsync(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((ProductDetailDto?)null);

            var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));

            Assert.Equal($"Product with Id {productId} not found.", exception.Message);

            _productQueryServiceMock.Verify(
                x => x.GetProductDetailAsync(productId, It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
