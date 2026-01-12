namespace Application.Features.Appsettings.v1.Commands.UpdateAppSetting
{
    public class UpdateAppSettingValidator : AbstractValidator<UpdateAppSettingCommand>
    {
        public UpdateAppSettingValidator()
        {
            RuleFor(x => x.Dto.Group)
                .NotEmpty().WithMessage("Group is required.")
                .MaximumLength(50);

            RuleFor(x => x.Dto.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(100);

            RuleFor(x => x.Dto.Value)
                .NotEmpty().WithMessage("Value cannot be empty.");

            RuleFor(x => x.Dto.Description)
                .MaximumLength(500);
        }
    }
}