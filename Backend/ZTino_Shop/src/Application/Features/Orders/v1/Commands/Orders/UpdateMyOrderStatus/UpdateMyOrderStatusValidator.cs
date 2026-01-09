using Domain.Constants;

namespace Application.Features.Orders.v1.Commands.Orders.UpdateMyOrderStatus
{
    /// <summary>
    /// Validator for user order status updates.
    /// Users can only set status to: Cancelled, Returned, or Delivered.
    /// </summary>
    public class UpdateMyOrderStatusValidator : AbstractValidator<UpdateMyOrderStatusCommand>
    {
        private static readonly HashSet<string> UserAllowedStatuses = new()
        {
            OrderStatus.Cancelled,
            OrderStatus.Returned,
            OrderStatus.Delivered
        };

        public UpdateMyOrderStatusValidator()
        {
            RuleFor(x => x.Dto.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required.");

            RuleFor(x => x.Dto.NewStatus)
                .NotEmpty()
                .WithMessage("NewStatus is required.")
                .Must(status => UserAllowedStatuses.Contains(status))
                .WithMessage("Users can only set status to: Cancelled, Returned, or Delivered.");

            RuleFor(x => x.Dto.CancelReason)
                .NotEmpty()
                .When(x => x.Dto.NewStatus == OrderStatus.Cancelled)
                .WithMessage("CancelReason is required when cancelling an order.");

            RuleFor(x => x.Dto.Note)
                .NotEmpty()
                .When(x => x.Dto.NewStatus == OrderStatus.Returned)
                .WithMessage("Note is required when requesting a return.");

            RuleFor(x => x.Dto.Note)
                .MaximumLength(500)
                .WithMessage("Note must not exceed 500 characters.");
        }
    }
}
