using Application.Features.Appsettings.v1.Commands.UpdateAppSetting;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.AppSettings
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/app-settings")]
    public class AppSettingsManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AppSettingsManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppSetting(UpdateAppSettingCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
