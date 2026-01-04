using Application.Common.Interfaces.Identity;
using Application.Features.Carts.v1.DTOs;
using Application.Features.Carts.v1.Services;

namespace Application.Features.Carts.v1.Queries.GetMyCart
{
    public class GetMyCartHandler : IRequestHandler<GetMyCartQuery, CartDto>
    {
        private readonly ICartQueryService _cartQueryService;
        private readonly ICurrentUser _currentUser;

        public GetMyCartHandler(
            ICartQueryService cartQueryService,
            ICurrentUser currentUser)
        {
            _cartQueryService = cartQueryService;
            _currentUser = currentUser;
        }

        public async Task<CartDto> Handle(GetMyCartQuery request, CancellationToken cancellationToken)
        {
            var userId = GetAndValidateUserId();

            var cart = await _cartQueryService.GetCartByUserIdAsync(userId, cancellationToken);

            return cart ?? CreateEmptyCart();
        }

        private Guid GetAndValidateUserId()
        {
            var userId = _currentUser.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("Authentication required to view your cart.");
            }

            return userId;
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
