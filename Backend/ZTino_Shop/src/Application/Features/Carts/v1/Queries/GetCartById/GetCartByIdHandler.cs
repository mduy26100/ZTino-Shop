using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Repositories;
using Application.Features.Carts.v1.Services;

namespace Application.Features.Carts.v1.Queries.GetCartById
{
    public class GetCartByIdHandler : IRequestHandler<GetCartByIdQuery, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartQueryService _cartQueryService;

        public GetCartByIdHandler(
            ICartRepository cartRepository,
            ICartQueryService cartQueryService)
        {
            _cartRepository = cartRepository;
            _cartQueryService = cartQueryService;
        }

        public async Task<CartDto> Handle(GetCartByIdQuery request, CancellationToken cancellationToken)
        {
            await ValidateGuestCartAccessAsync(request.CartId, cancellationToken);

            var cart = await _cartQueryService.GetCartByIdAsync(request.CartId, cancellationToken);

            return cart ?? CreateEmptyCart();
        }

        private async Task ValidateGuestCartAccessAsync(Guid cartId, CancellationToken cancellationToken)
        {
            var cart = await _cartRepository.FindOneAsync(
                c => c.Id == cartId && c.IsActive,
                asNoTracking: true,
                cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException($"Cart with ID {cartId} not found.");
            }

            if (cart.UserId.HasValue)
            {
                throw new ForbiddenException("This cart belongs to a registered user. Please login to access it.");
            }
        }

        private static CartDto CreateEmptyCart()
        {
            return new CartDto
            {
                CartId = Guid.Empty,
                Items = new List<CartItemDto>(),
                TotalPrice = 0,
                TotalItems = 0
            };
        }
    }
}
