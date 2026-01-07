using Application.Features.Orders.v1.DTOs;

namespace Application.Features.Orders.v1.Services
{
    public interface IOrderQueryService
    {
        Task<OrderLookupResponseDto?> GetGuestOrderByCodeAndPhoneAsync(string orderCode, string phoneNumber, CancellationToken cancellationToken = default);
    }
}