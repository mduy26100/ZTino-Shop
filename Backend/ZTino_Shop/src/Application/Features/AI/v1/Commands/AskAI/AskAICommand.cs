namespace Application.Features.AI.v1.Commands.AskAI
{
    public record AskAICommand(
        string Prompt,
        string? SessionId = null
    ) : IRequest<string>;
}
