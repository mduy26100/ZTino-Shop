namespace Application.Common.Abstractions.ExternalServices.AI
{
    public interface IAIFactory
    {
        Task<IAIService> GetActiveServiceAsync(CancellationToken cancellationToken);
    }
}
