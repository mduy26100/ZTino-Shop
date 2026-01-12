using Application.Common.Abstractions.ExternalServices.AI;
using Application.Common.Abstractions.Security;
using Application.Features.Appsettings.v1.Repositories;
using Domain.Consts;
using Infrastructure.ExternalServices.AI.Models;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices.AI
{
    public class GeminiService : IAIService
    {
        public string ProviderName => AppSettingConstants.AIProviders.Gemini;

        private readonly HttpClient _httpClient;
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IEncryptionService _encryptionService;

        public GeminiService(
            HttpClient httpClient,
            IAppSettingRepository appSettingRepository,
            IEncryptionService encryptionService)
        {
            _httpClient = httpClient;
            _appSettingRepository = appSettingRepository;
            _encryptionService = encryptionService;
        }

        public async Task<string> GenerateContentAsync(string prompt, CancellationToken cancellationToken)
        {
            var setting = await _appSettingRepository.GetByGroupAndKeyAsync(
                AppSettingConstants.Gemini.Group,
                AppSettingConstants.Gemini.Keys.FlashApiKey,
                cancellationToken);

            if (setting == null || string.IsNullOrWhiteSpace(setting.Value))
            {
                throw new InvalidOperationException($"Gemini API Key is missing in AppSettings (Group: {AppSettingConstants.System.Group}).");
            }

            var apiKey = _encryptionService.Decrypt(setting.Value);

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException("Decrypted Gemini API Key is empty.");
            }

            var requestBody = new GeminiRequest
            {
                Contents = new List<RequestContent>
                {
                    new RequestContent
                    {
                        Parts = new List<RequestPart>
                        {
                            new RequestPart { Text = prompt }
                        }
                    }
                }
            };

            var url = $"{AppSettingConstants.Gemini.BaseUrl}?key={apiKey}";

            var response = await _httpClient.PostAsJsonAsync(url, requestBody, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"Gemini API Failed. Status: {response.StatusCode}. Details: {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<GeminiResponse>(cancellationToken: cancellationToken);

            var answer = result?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;

            return answer ?? "AI did not return any content.";
        }
    }
}