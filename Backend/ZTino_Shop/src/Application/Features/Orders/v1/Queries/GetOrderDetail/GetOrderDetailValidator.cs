namespace Application.Features.Orders.v1.Queries.GetOrderDetail
{
    public class GetOrderDetailValidator : AbstractValidator<GetOrderDetailQuery>
    {
        public GetOrderDetailValidator()
        {
            RuleFor(x => x.orderCode)
                   .NotEmpty().WithMessage("Order code is required.")
                   .MaximumLength(30).WithMessage("Order code must not exceed 30 characters.");
        }
    }
}
