using Application.Features.Products.v1.Commands.Colors.UpdateColor;
using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.Colors.UpdateColor
{
    public class UpdateColorHandlerTests
    {
        private readonly Mock<IColorRepository> _colorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;
        private readonly UpdateColorHandler _handler;

        public UpdateColorHandlerTests()
        {
            _colorRepositoryMock = new Mock<IColorRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new UpdateColorHandler(
                _colorRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateColor_WhenValidRequest()
        {
            var dto = new UpsertColorDto { Id = 1, Name = "Blue" };
            var command = new UpdateColorCommand(dto);

            var existingEntity = new Color { Id = 1, Name = "OldName" };

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            _colorRepositoryMock
                .Setup(r => r.AnyAsync(
                    It.IsAny<Expression<Func<Color, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
               .Setup(m => m.Map<UpsertColorDto>(existingEntity))
               .Returns(dto);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Blue", result.Name);

            _colorRepositoryMock.Verify(r => r.Update(existingEntity), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFound_WhenColorDoesNotExist()
        {
            var dto = new UpsertColorDto { Id = 1, Name = "Blue" };
            var command = new UpdateColorCommand(dto);

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Color?)null);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _colorRepositoryMock.Verify(r => r.Update(It.IsAny<Color>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperation_WhenNameAlreadyExists()
        {
            var dto = new UpsertColorDto { Id = 1, Name = "Red" };
            var command = new UpdateColorCommand(dto);

            var existingEntity = new Color { Id = 1, Name = "OldName" };

            _colorRepositoryMock
                .Setup(r => r.GetByIdAsync(dto.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingEntity);

            _colorRepositoryMock
                .Setup(r => r.AnyAsync(
                    It.IsAny<Expression<Func<Color, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None));

            _colorRepositoryMock.Verify(r => r.Update(It.IsAny<Color>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
