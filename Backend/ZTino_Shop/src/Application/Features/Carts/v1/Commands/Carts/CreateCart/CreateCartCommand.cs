using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Commands.Carts.CreateCart
{
    public record CreateCartCommand(UpsertCartDto Dto) : IRequest<UpsertCartResponseDto>;
}
