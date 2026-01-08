using Domain.Constants;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateOrderStatus
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        private static readonly HashSet<string> ValidStatuses = new()
        {
            OrderStatus.Pending,
            OrderStatus.Confirmed,
            OrderStatus.Shipping,
            OrderStatus.Delivered,
            OrderStatus.Cancelled,
            OrderStatus.Returned
        };

        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Dto.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required.");

            RuleFor(x => x.Dto.NewStatus)
                .NotEmpty()
                .WithMessage("NewStatus is required.")
                .Must(status => ValidStatuses.Contains(status))
                .WithMessage("Invalid order status.");

            RuleFor(x => x.Dto.CancelReason)
                .NotEmpty()
                .When(x => x.Dto.NewStatus == OrderStatus.Cancelled)
                .WithMessage("CancelReason is required when cancelling an order.");

            RuleFor(x => x.Dto.Note)
                .MaximumLength(500)
                .WithMessage("Note must not exceed 500 characters.");
        }
    }
}
