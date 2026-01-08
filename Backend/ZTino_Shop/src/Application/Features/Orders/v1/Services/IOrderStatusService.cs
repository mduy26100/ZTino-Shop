using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Services
{
    public interface IOrderStatusService
    {
        void ValidateStatusTransition(string currentStatus, string newStatus);
        Task ProcessStatusChangeAsync(Order order, string newStatus, CancellationToken cancellationToken = default);
    }
}