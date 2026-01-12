using Application.Features.Appsettings.v1.DTOs;

namespace Application.Features.Appsettings.v1.Commands.UpdateAppSetting
{
    public record UpdateAppSettingCommand(UpdateAppSettingDto Dto) : IRequest<bool>;
}