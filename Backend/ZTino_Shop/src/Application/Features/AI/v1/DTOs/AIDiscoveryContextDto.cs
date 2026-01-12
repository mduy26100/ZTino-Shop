namespace Application.Features.AI.v1.DTOs
{
    public class AIDiscoveryContextDto
    {
        public List<string> Categories { get; set; } = new();
        public List<AIProductContextDto> FeaturedProducts { get; set; } = new();
    }
}
