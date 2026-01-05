namespace Application.Features.Orders.v1.Commands.Orders.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Dto.CartId)
                .NotEmpty()
                .WithMessage("CartId is required.");

            RuleFor(x => x.Dto.SelectedCartItemIds)
                .NotEmpty()
                .WithMessage("Please select at least one item to order.");

            RuleFor(x => x.Dto.CustomerName)
                .NotEmpty()
                .WithMessage("Customer name is required.")
                .MaximumLength(100)
                .WithMessage("Customer name must not exceed 100 characters.");

            RuleFor(x => x.Dto.CustomerPhone)
                .NotEmpty()
                .WithMessage("Customer phone is required.")
                .Matches(@"^(0|\+84)[0-9]{9,10}$")
                .WithMessage("Invalid phone number format. Expected: 0xxxxxxxxx or +84xxxxxxxxx");

            RuleFor(x => x.Dto.CustomerEmail)
                .EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Dto.CustomerEmail))
                .WithMessage("Invalid email format.");

            RuleFor(x => x.Dto.ShippingAddress)
                .NotNull()
                .WithMessage("Shipping address is required.");

            When(x => x.Dto.ShippingAddress is not null, () =>
            {
                RuleFor(x => x.Dto.ShippingAddress.RecipientName)
                    .NotEmpty()
                    .WithMessage("Recipient name is required.")
                    .MaximumLength(100)
                    .WithMessage("Recipient name must not exceed 100 characters.");

                RuleFor(x => x.Dto.ShippingAddress.PhoneNumber)
                    .NotEmpty()
                    .WithMessage("Recipient phone number is required.")
                    .Matches(@"^(0|\+84)[0-9]{9,10}$")
                    .WithMessage("Invalid recipient phone number format.");

                RuleFor(x => x.Dto.ShippingAddress.Street)
                    .NotEmpty()
                    .WithMessage("Street address is required.")
                    .MaximumLength(200)
                    .WithMessage("Street address must not exceed 200 characters.");

                RuleFor(x => x.Dto.ShippingAddress.Ward)
                    .NotEmpty()
                    .WithMessage("Ward is required.")
                    .MaximumLength(100)
                    .WithMessage("Ward must not exceed 100 characters.");

                RuleFor(x => x.Dto.ShippingAddress.District)
                    .NotEmpty()
                    .WithMessage("District is required.")
                    .MaximumLength(100)
                    .WithMessage("District must not exceed 100 characters.");

                RuleFor(x => x.Dto.ShippingAddress.City)
                    .NotEmpty()
                    .WithMessage("City is required.")
                    .MaximumLength(100)
                    .WithMessage("City must not exceed 100 characters.");
            });

            RuleFor(x => x.Dto.Note)
                .MaximumLength(500)
                .WithMessage("Note must not exceed 500 characters.");
        }
    }
}
