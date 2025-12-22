using Application.Common.Interfaces.Persistence.EFCore;
using Application.Features.Products.Commands.Sizes.UpdateSize;
using Application.Features.Products.DTOs.Sizes;
using Application.Features.Products.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.Sizes.UpdateSize
{
    public class UpdateSizeHandlerTests
    {
        private readonly Mock<ISizeRepository> _sizeRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateSizeHandler _handler;

        public UpdateSizeHandlerTests()
        {
            _sizeRepositoryMock = new Mock<ISizeRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateSizeHandler(
                _sizeRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateSize_WhenDataIsValid()
        {
            var dto = new UpsertSizeDto { Id = 1, Name = "Updated Size" };
            var command = new UpdateSizeCommand(dto);
            var entity = new Size { Id = 1, Name = "Old Size" };

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _sizeRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(x => x.Map(dto, entity));

            _mapperMock
                .Setup(x => x.Map<UpsertSizeDto>(entity))
                .Returns(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);

            _sizeRepositoryMock.Verify(x => x.Update(entity), Times.Once);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenSizeNotFound()
        {
            var dto = new UpsertSizeDto { Id = 999, Name = "Any Name" };
            var command = new UpdateSizeCommand(dto);

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Size?)null);

            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal($"Size with Id {dto.Id} not found.", exception.Message);

            _sizeRepositoryMock.Verify(x => x.Update(It.IsAny<Size>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_WhenNameAlreadyExists()
        {
            var dto = new UpsertSizeDto { Id = 1, Name = "Duplicate Name" };
            var command = new UpdateSizeCommand(dto);
            var entity = new Size { Id = 1, Name = "Old Name" };

            _sizeRepositoryMock
                .Setup(x => x.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            _sizeRepositoryMock
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<Size, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Size with the same name already exists.", exception.Message);

            _sizeRepositoryMock.Verify(x => x.Update(It.IsAny<Size>()), Times.Never);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
