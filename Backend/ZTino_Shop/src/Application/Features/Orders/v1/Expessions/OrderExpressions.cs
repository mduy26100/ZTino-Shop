using System.Linq.Expressions;
using Application.Features.Orders.v1.DTOs;
using Domain.Models.Orders;

namespace Application.Features.Orders.v1.Expressions
{
    public static class OrderExpressions
    {
        public static Expression<Func<Order, OrderLookupResponseDto>> OrderLookupProjection => order => new OrderLookupResponseDto
        {
            Id = order.Id,
            OrderCode = order.OrderCode,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            PaymentMethod = order.PaymentMethod,

            CustomerName = order.CustomerName,
            CustomerPhone = order.CustomerPhone,
            ShippingAddress = order.ShippingAddress != null ? order.ShippingAddress.FullAddress : string.Empty,
            Note = order.Note,

            SubTotal = order.SubTotal,
            ShippingFee = order.ShippingFee,
            DiscountAmount = order.DiscountAmount,
            TotalAmount = order.TotalAmount,

            Items = order.OrderItems.Select(item => new OrderLookupItemDto
            {
                ProductName = item.ProductName,
                Sku = item.Sku,
                ColorName = item.ColorName,
                SizeName = item.SizeName,
                ThumbnailUrl = item.ThumbnailUrl,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                TotalLineAmount = item.TotalLineAmount
            }).ToList(),

            Histories = order.OrderHistory
                .AsQueryable()
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => new OrderLookupHistoryDto
                {
                    Status = h.Status,
                    Note = h.Note,
                    CreatedAt = h.CreatedAt
                }).ToList()
        };
    }
}