using Domain.Models.Products;
using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Repositories;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies;

namespace Application.Tests.Products.ProductImages.UpdateProductImage.Strategies
{
    public class ClaimMainStrategyTests
    {
        private readonly Mock<IProductImageRepository> _repoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly ClaimMainStrategy _strategy;

        public ClaimMainStrategyTests()
        {
            _repoMock = new Mock<IProductImageRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _strategy = new ClaimMainStrategy();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldDisableOldMain_AndEnableNewMain()
        {
            var entity = new ProductImage { Id = 1, ProductVariantId = 10, IsMain = false };
            var oldMain = new ProductImage { Id = 2, ProductVariantId = 10, IsMain = true };

            _repoMock.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<ProductImage, bool>>>(),
                true,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage> { oldMain });

            await _strategy.ExecuteAsync(entity, _repoMock.Object, _contextMock.Object, CancellationToken.None);

            Assert.False(oldMain.IsMain);
            _repoMock.Verify(x => x.Update(oldMain), Times.Once);

            Assert.True(entity.IsMain);
            _repoMock.Verify(x => x.Update(entity), Times.Once);

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ExecuteAsync_ShouldJustEnableNewMain_WhenNoOldMainExists()
        {
            var entity = new ProductImage { Id = 1, ProductVariantId = 10, IsMain = false };

            _repoMock.Setup(x => x.FindAsync(
                It.IsAny<Expression<Func<ProductImage, bool>>>(),
                true,
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ProductImage>());

            await _strategy.ExecuteAsync(entity, _repoMock.Object, _contextMock.Object, CancellationToken.None);

            Assert.True(entity.IsMain);

            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}