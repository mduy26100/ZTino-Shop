namespace Application.Features.Orders.v1.Queries.GetOrderLookup
{
    public class GetOrderLookupValidator : AbstractValidator<GetOrderLookupQuery>
    {
        public GetOrderLookupValidator()
        {
            RuleFor(x => x.Dto).NotNull().WithMessage("Request data is required.");

            RuleSet("DtoFields", () => {
                RuleFor(x => x.Dto.OrderCode)
                    .NotEmpty().WithMessage("Order code is required.")
                    .MaximumLength(30).WithMessage("Order code must not exceed 30 characters.");

                RuleFor(x => x.Dto.CustomerPhone)
                    .NotEmpty()
                    .WithMessage("Customer phone is required.")
                    .Matches(@"^(0|\+84)[0-9]{9,10}$")
                    .WithMessage("Invalid phone number format. Expected: 0xxxxxxxxx or +84xxxxxxxxx");
            });
        }
    }
}