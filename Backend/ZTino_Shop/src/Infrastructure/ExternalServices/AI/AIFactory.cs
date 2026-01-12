using Application.Common.Abstractions.ExternalServices.AI;
using Application.Common.Abstractions.Security;
using Application.Common.Exceptions;
using Application.Features.Appsettings.v1.Repositories;
using Domain.Consts;

namespace Infrastructure.ExternalServices.AI
{
    public class AIFactory : IAIFactory
    {
        private readonly IEnumerable<IAIService> _services;
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IEncryptionService _encryptionService;

        public AIFactory(
            IEnumerable<IAIService> services,
            IAppSettingRepository appSettingRepository,
            IEncryptionService encryptionService)
        {
            _services = services;
            _appSettingRepository = appSettingRepository;
            _encryptionService = encryptionService;
        }

        public async Task<IAIService> GetActiveServiceAsync(CancellationToken cancellationToken)
        {
            var setting = await _appSettingRepository.GetByGroupAndKeyAsync(
                AppSettingConstants.System.Group,
                AppSettingConstants.System.Keys.ActiveAIProvider,
                cancellationToken);

            string activeProvider;

            if (setting != null && !string.IsNullOrWhiteSpace(setting.Value))
            {
                activeProvider = _encryptionService.Decrypt(setting.Value);
            }
            else
            {
                activeProvider = AppSettingConstants.AIProviders.Gemini;
            }

            var service = _services.FirstOrDefault(x => x.ProviderName == activeProvider);

            if (service == null)
            {
                throw new NotFoundException($"System Error: AI Provider '{activeProvider}' not found or not implemented.");
            }

            return service;
        }
    }
}