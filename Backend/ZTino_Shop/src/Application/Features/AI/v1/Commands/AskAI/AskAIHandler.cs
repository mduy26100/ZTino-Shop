using Application.Common.Abstractions.ExternalServices.AI;

namespace Application.Features.AI.v1.Commands.AskAI
{
    public class AskAIHandler : IRequestHandler<AskAICommand, string>
    {
        private readonly IAIFactory _aiFactory;

        public AskAIHandler(IAIFactory aiFactory)
        {
            _aiFactory = aiFactory;
        }

        public async Task<string> Handle(AskAICommand request, CancellationToken cancellationToken)
        {
            var aiService = await _aiFactory.GetActiveServiceAsync(cancellationToken);

            var result = await aiService.GenerateContentAsync(request.Prompt, cancellationToken);

            return result;
        }
    }
}
