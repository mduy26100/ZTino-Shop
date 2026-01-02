using Application.Features.Products.v1.DTOs.Products;
using Application.Features.Products.v1.Queries.Products.GetProductsByCategoryId;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Products.GetProductsByCategoryId
{
    public class GetProductsByCategoryIdHandlerTests
    {
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly GetProductsByCategoryIdHandler _handler;

        public GetProductsByCategoryIdHandlerTests()
        {
            _productRepoMock = new Mock<IProductRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetProductsByCategoryIdHandler(
                _productRepoMock.Object,
                _categoryRepoMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_NoCategoryFound_ReturnEmptyList()
        {
            int CategoryId = 1;
            var query = new GetProductsByCategoryIdQuery(CategoryId);

            _categoryRepoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Category>());

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Empty(result);

            _productRepoMock.Verify(
                x => x.FindAsync(It.IsAny<Expression<Func<Product, bool>>>(), true, It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Fact]
        public async Task Handle_CategoryAndSubCategory_ReturnProductsWithCategoryName()
        {
            int CategoryId = 1;
            var query = new GetProductsByCategoryIdQuery(CategoryId);

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Parent" },
                new Category { Id = 2, Name = "Child", ParentId = 1 }
            };

            var products = new List<Product>
            {
                new Product { Id = 10, CategoryId = 1 },
                new Product { Id = 20, CategoryId = 2 }
            };

            var productDtos = new List<ProductSummaryDto>
            {
                new ProductSummaryDto { Id = 10, CategoryId = 1 },
                new ProductSummaryDto { Id = 20, CategoryId = 2 }
            };

            _categoryRepoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            _productRepoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock
                .Setup(x => x.Map<List<ProductSummaryDto>>(products))
                .Returns(productDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(2, result.Count);

            var parentProduct = result.First(x => x.CategoryId == 1);
            var childProduct = result.First(x => x.CategoryId == 2);

            Assert.Equal("Parent", parentProduct.CategoryName);
            Assert.Equal("Child", childProduct.CategoryName);
        }

        [Fact]
        public async Task Handle_ReturnProductsSortedByIdDescending()
        {
            int CategoryId = 1;
            var query = new GetProductsByCategoryIdQuery(CategoryId);

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category" }
            };

            var products = new List<Product>
            {
                new Product { Id = 1, CategoryId = 1 },
                new Product { Id = 3, CategoryId = 1 },
                new Product { Id = 2, CategoryId = 1 }
            };

            var productDtos = new List<ProductSummaryDto>
            {
                new ProductSummaryDto { Id = 1, CategoryId = 1 },
                new ProductSummaryDto { Id = 3, CategoryId = 1 },
                new ProductSummaryDto { Id = 2, CategoryId = 1 }
            };

            _categoryRepoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            _productRepoMock
                .Setup(x => x.FindAsync(
                    It.IsAny<Expression<Func<Product, bool>>>(),
                    true,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _mapperMock
                .Setup(x => x.Map<List<ProductSummaryDto>>(products))
                .Returns(productDtos);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.Equal(new[] { 3, 2, 1 }, result.Select(x => x.Id));
        }
    }
}
