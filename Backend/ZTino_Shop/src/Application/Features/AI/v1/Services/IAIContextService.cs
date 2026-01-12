using Application.Features.AI.v1.DTOs;

namespace Application.Features.AI.v1.Services
{
    public interface IAIContextService
    {
        Task<List<AIProductContextDto>> GetProductContextAsync(
            ExtractedKeywords keywords,
            CancellationToken cancellationToken = default);

        Task<AIDiscoveryContextDto> GetDiscoveryContextAsync(
            CancellationToken cancellationToken = default);
    }
}
