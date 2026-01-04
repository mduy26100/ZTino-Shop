using Application.Features.Carts.v1.DTOs;

namespace Application.Features.Carts.v1.Commands.Carts.UpdateCart
{
    public record UpdateCartCommand(UpsertCartDto Dto) : IRequest<UpsertCartResponseDto>;
}
