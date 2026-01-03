using Application.Features.Products.v1.DTOs.ProductImages;

namespace Application.Features.Products.v1.Commands.ProductImages.CreateProductImages
{
    public class CreateProductImagesValidator : AbstractValidator<CreateProductImagesCommand>
    {
        public CreateProductImagesValidator()
        {
            RuleFor(x => x.Dtos)
                .NotEmpty().WithMessage("Product images list must not be empty.")
                .Must(HaveSameVariantId).WithMessage("All images must belong to the same Product Variant.");

            RuleForEach(x => x.Dtos).ChildRules(dto =>
            {
                dto.RuleFor(x => x.ProductColorId)
                    .GreaterThan(0).WithMessage("ProductVariantId must be greater than 0.");

                dto.RuleFor(x => x.ImageUrl)
                   .MaximumLength(500).WithMessage("ImageUrl must not exceed 500 characters.")
                   .When(x => !string.IsNullOrWhiteSpace(x.ImageUrl));

                dto.RuleFor(x => x)
                    .Must(x => x.ImgContent != null || !string.IsNullOrWhiteSpace(x.ImageUrl))
                    .WithMessage("Either an Image File or an Image URL must be provided.");
            });
        }

        private bool HaveSameVariantId(List<UpsertProductImageDto> dtos)
        {
            if (dtos is null || dtos.Count == 0) return true;
            var firstId = dtos[0].ProductColorId;
            return dtos.All(x => x.ProductColorId == firstId);
        }
    }
}