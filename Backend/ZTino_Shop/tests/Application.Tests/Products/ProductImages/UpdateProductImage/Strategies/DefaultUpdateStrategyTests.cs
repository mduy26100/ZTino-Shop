using Domain.Models.Products;
using Application.Common.Interfaces.Persistence.Data;
using Application.Features.Products.Repositories;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage.Strategies;

namespace Application.Tests.Products.ProductImages.UpdateProductImage.Strategies
{
    public class DefaultUpdateStrategyTests
    {
        private readonly Mock<IProductImageRepository> _repoMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly DefaultUpdateStrategy _strategy;

        public DefaultUpdateStrategyTests()
        {
            _repoMock = new Mock<IProductImageRepository>();
            _contextMock = new Mock<IApplicationDbContext>();
            _strategy = new DefaultUpdateStrategy();
        }

        [Fact]
        public async Task ExecuteAsync_ShouldUpdateAndSave()
        {
            var entity = new ProductImage { Id = 1, IsMain = false };

            _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            await _strategy.ExecuteAsync(entity, _repoMock.Object, _contextMock.Object, CancellationToken.None);

            _repoMock.Verify(x => x.Update(entity), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}