namespace Application.Features.Carts.v1.Commands.Carts.DeleteCart
{
    public record DeleteCartCommand(int CartItemId) : IRequest;
}
