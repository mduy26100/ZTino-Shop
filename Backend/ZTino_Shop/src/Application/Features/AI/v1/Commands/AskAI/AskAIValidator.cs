namespace Application.Features.AI.v1.Commands.AskAI
{
    public class AskAIValidator : AbstractValidator<AskAICommand>
    {
        public AskAIValidator()
        {
            RuleFor(x => x.Prompt)
                .NotEmpty()
                .WithMessage("The prompt cannot be empty.");
        }
    }
}
