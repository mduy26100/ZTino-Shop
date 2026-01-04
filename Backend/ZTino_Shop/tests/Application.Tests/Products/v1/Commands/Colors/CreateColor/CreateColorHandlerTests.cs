using Application.Features.Products.v1.Commands.Colors.CreateColor;
using Application.Features.Products.v1.DTOs.Colors;
using Application.Features.Products.v1.Repositories;
using Domain.Models.Products;

namespace Application.Tests.Products.v1.Commands.Colors.CreateColor
{
    public class CreateColorHandlerTests
    {
        private readonly Mock<IColorRepository> _colorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IApplicationDbContext> _contextMock;

        private readonly CreateColorHandler _handler;

        public CreateColorHandlerTests()
        {
            _colorRepositoryMock = new Mock<IColorRepository>();
            _mapperMock = new Mock<IMapper>();
            _contextMock = new Mock<IApplicationDbContext>();

            _handler = new CreateColorHandler(
                _colorRepositoryMock.Object,
                _mapperMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateColor_WhenNameDoesNotExist()
        {
            var dto = new UpsertColorDto { Name = "Red" };
            var command = new CreateColorCommand(dto);

            var colorEntity = new Color { Id = 1, Name = "Red" };

            _colorRepositoryMock
                .Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Color, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Color>(dto))
                .Returns(colorEntity);

            _mapperMock
                .Setup(m => m.Map<UpsertColorDto>(colorEntity))
                .Returns(new UpsertColorDto
                {
                    Id = 1,
                    Name = "Red"
                });

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Red", result.Name);

            _colorRepositoryMock.Verify(r => r.AddAsync(colorEntity, It.IsAny<CancellationToken>()), Times.Once);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


        [Fact]
        public async Task Handle_ShouldThrowException_WhenNameAlreadyExists()
        {
            var dto = new UpsertColorDto { Name = "Red" };
            var command = new CreateColorCommand(dto);

            _colorRepositoryMock
                .Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<Color, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ConflictException>(() =>
                _handler.Handle(command, CancellationToken.None)
            );

            _colorRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Color>(), It.IsAny<CancellationToken>()), Times.Never);
            _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
