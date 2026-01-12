namespace Application.Features.AI.v1.Commands.AskAI
{
    public record AskAICommand(string Prompt) : IRequest<string>;
}
