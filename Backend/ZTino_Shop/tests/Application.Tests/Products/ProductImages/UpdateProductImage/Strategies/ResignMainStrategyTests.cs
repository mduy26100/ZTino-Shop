using Domain.Models.Products;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies;

namespace Application.Tests.Products.ProductImages.UpdateProductImage.Strategies
{
    public class ResignMainStrategyTests
    {
        private readonly Mock<IProductImageRepository> _repoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly ResignMainStrategy _strategy;

        public ResignMainStrategyTests()
        {
            _repoMock = new Mock<IProductImageRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _strategy = new ResignMainStrategy();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldPromoteHeir_WhenHeirExists()
        {
            var entity = new ProductImage { Id = 1, ProductVariantId = 10, IsMain = true };
            var heir = new ProductImage { Id = 2, ProductVariantId = 10, IsMain = false };

            _repoMock.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<ProductImage, bool>>>(),
                true,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage> { heir });

            await _strategy.ExecuteAsync(entity, _repoMock.Object, _contextMock.Object, CancellationToken.None);

            Assert.False(entity.IsMain);
            _repoMock.Verify(x => x.Update(entity), Times.Once);

            Assert.True(heir.IsMain);
            _repoMock.Verify(x => x.Update(heir), Times.Once);

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldRevertToMain_WhenNoHeirExists()
        {
            var entity = new ProductImage { Id = 1, ProductVariantId = 10, IsMain = true };

            _repoMock.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<ProductImage, bool>>>(),
                true,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            await _strategy.ExecuteAsync(entity, _repoMock.Object, _contextMock.Object, CancellationToken.None);

            Assert.True(entity.IsMain);

            _repoMock.Verify(x => x.Update(entity), Times.Exactly(2));

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}