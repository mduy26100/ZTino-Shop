using Application.Common.Abstractions.Security;
using Application.Features.Appsettings.v1.Repositories;

namespace Application.Features.Appsettings.v1.Commands.UpdateAppSetting
{
    public class UpdateAppSettingHandler : IRequestHandler<UpdateAppSettingCommand, bool>
    {
        private readonly IAppSettingRepository _appSettingRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IApplicationDbContext _context;

        public UpdateAppSettingHandler(IAppSettingRepository appSettingRepository,
            IEncryptionService encryptionService,
            IApplicationDbContext context)
        {
            _appSettingRepository = appSettingRepository;
            _encryptionService = encryptionService;
            _context = context;
        }

        public async Task<bool> Handle(UpdateAppSettingCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var setting = await _appSettingRepository.FindOneAsync(a => a.Group == dto.Group && a.Key == dto.Key, false, cancellationToken);
            if(setting is null)
            {
                throw new NotFoundException($"AppSetting with Group '{dto.Group}' and Key '{dto.Key}' not found.");
            }

            var encryptedValue = _encryptionService.Encrypt(dto.Value);

            setting.Value = encryptedValue;
            setting.Description = dto.Description;
            setting.IsActive = dto.IsActive;

            _appSettingRepository.Update(setting);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
