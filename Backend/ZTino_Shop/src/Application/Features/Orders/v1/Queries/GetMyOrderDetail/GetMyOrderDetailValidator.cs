namespace Application.Features.Orders.v1.Queries.GetMyOrderDetail
{
    public class GetMyOrderDetailValidator : AbstractValidator<GetMyOrderDetailQuery>
    {
        public GetMyOrderDetailValidator()
        {
            RuleFor(x => x.orderCode)
                    .NotEmpty().WithMessage("Order code is required.")
                    .MaximumLength(30).WithMessage("Order code must not exceed 30 characters.");
        }
    }
}
