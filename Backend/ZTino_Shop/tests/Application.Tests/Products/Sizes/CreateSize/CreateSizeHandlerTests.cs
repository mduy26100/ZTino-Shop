using Application.Features.Products.v1.Commands.Sizes.CreateSize;
using Application.Features.Products.v1.DTOs.Sizes;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Sizes.CreateSize
{
    public class CreateSizeHandlerTests
    {
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly CreateSizeHandler _handler;

        public CreateSizeHandlerTests()
        {
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new CreateSizeHandler(
                _sizeRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_CreateSize_WhenNameIsUnique()
        {
            var dto = new UpsertSizeDto { Id = 0, Name = "Large" };
            var command = new CreateSizeCommand(dto);
            var entity = new Size { Id = 1, Name = "Large" };

            _sizeRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(x => x.Map<Size>(dto)).Returns(entity);
            _mapperMock.Setup(x => x.Map<UpsertSizeDto>(entity)).Returns(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);

            _sizeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Size>(), It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenNameAlreadyExists()
        {
            var dto = new UpsertSizeDto { Name = "Existing Name" };
            var command = new CreateSizeCommand(dto);

            _sizeRepositoryMock.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Size with the same name already exists.", exception.Message);

            _sizeRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Size>(), It.IsAny<CancellationToken>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}