using Application.Common.Exceptions;
using Application.Features.Finances.v1.Services;
using Application.Features.Orders.v1.Repositories;
using Application.Features.Orders.v1.Services;
using Application.Features.Products.v1.Services;
using Application.Features.Stats.v1.Services;
using Domain.Constants;
using Domain.Models.Orders;

namespace Infrastructure.Orders.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly IOrderPaymentRepository _orderPaymentRepository;
        private readonly IInvoiceService _invoiceService;
        private readonly ISalesStatsService _salesStatsService;
        private readonly IStockService _stockService;

        private static readonly Dictionary<string, HashSet<string>> AllowedTransitions = new()
        {
            { OrderStatus.Pending, new HashSet<string> { OrderStatus.Confirmed, OrderStatus.Cancelled } },
            { OrderStatus.Confirmed, new HashSet<string> { OrderStatus.Shipping, OrderStatus.Cancelled } },
            { OrderStatus.Shipping, new HashSet<string> { OrderStatus.Delivered, OrderStatus.Returned } },
            { OrderStatus.Delivered, new HashSet<string> { OrderStatus.Returned } },
            { OrderStatus.Cancelled, new HashSet<string>() },
            { OrderStatus.Returned, new HashSet<string>() }
        };

        public OrderStatusService(
            IOrderPaymentRepository orderPaymentRepository,
            IInvoiceService invoiceService,
            ISalesStatsService salesStatsService,
            IStockService stockService)
        {
            _orderPaymentRepository = orderPaymentRepository;
            _invoiceService = invoiceService;
            _salesStatsService = salesStatsService;
            _stockService = stockService;
        }

        public void ValidateStatusTransition(string currentStatus, string newStatus)
        {
            if (currentStatus == newStatus)
            {
                throw new BusinessRuleException($"Order is already in '{currentStatus}' status.");
            }

            if (!AllowedTransitions.TryGetValue(currentStatus, out var allowed) || !allowed.Contains(newStatus))
            {
                throw new BusinessRuleException(
                    $"Cannot transition order from '{currentStatus}' to '{newStatus}'. " +
                    $"Allowed transitions: {string.Join(", ", AllowedTransitions.GetValueOrDefault(currentStatus, new HashSet<string>()))}");
            }
        }

        public async Task ProcessStatusChangeAsync(Order order, string newStatus, CancellationToken cancellationToken = default)
        {
            switch (newStatus)
            {
                case var s when s == OrderStatus.Delivered:
                    await ProcessDeliveryAsync(order, cancellationToken);
                    break;

                case var s when s == OrderStatus.Cancelled:
                    await ProcessCancellationAsync(order, cancellationToken);
                    break;

                case var s when s == OrderStatus.Returned:
                    await ProcessReturnAsync(order, cancellationToken);
                    break;
            }
        }

        private async Task ProcessDeliveryAsync(Order order, CancellationToken cancellationToken)
        {
            if (order.PaymentMethod == PaymentMethod.COD)
            {
                order.PaymentStatus = PaymentStatus.Completed;
            }

            await UpsertOrderPaymentAsync(order, PaymentStatus.Completed, cancellationToken);

            await _invoiceService.UpsertInvoiceAsync(order, cancellationToken);

            await _salesStatsService.UpdateDailyRevenueStatsAsync(order, cancellationToken);
            await _salesStatsService.UpdateProductSalesStatsAsync(order.OrderItems, cancellationToken);
        }

        private async Task ProcessCancellationAsync(Order order, CancellationToken cancellationToken)
        {
            order.PaymentStatus = PaymentStatus.Failed;

            UpdateExistingPaymentStatus(order);

            await _stockService.RestoreStockAsync(order.OrderItems, cancellationToken);
        }

        private async Task ProcessReturnAsync(Order order, CancellationToken cancellationToken)
        {
            order.PaymentStatus = PaymentStatus.Refunded;

            UpdateExistingPaymentStatus(order);

            await _stockService.RestoreStockAsync(order.OrderItems, cancellationToken);
        }

        private static void UpdateExistingPaymentStatus(Order order)
        {
            var existingPayment = order.Payments.FirstOrDefault(p => p.Method == order.PaymentMethod);
            if (existingPayment != null)
            {
                existingPayment.Status = order.PaymentStatus;
            }
        }

        private async Task UpsertOrderPaymentAsync(Order order, string paymentStatus, CancellationToken cancellationToken)
        {
            var existingPayment = order.Payments.FirstOrDefault(p => p.Method == order.PaymentMethod);

            if (existingPayment != null)
            {
                existingPayment.Status = paymentStatus;
                existingPayment.Amount = order.TotalAmount;
            }
            else
            {
                var payment = new OrderPayment
                {
                    OrderId = order.Id,
                    Method = order.PaymentMethod,
                    Status = paymentStatus,
                    Amount = order.TotalAmount,
                    CreatedAt = DateTime.UtcNow
                };

                await _orderPaymentRepository.AddAsync(payment, cancellationToken);
            }
        }
    }
}
