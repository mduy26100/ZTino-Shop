using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.CurrentUser;
using WebAPI.Requests.Auth;

namespace WebAPI.Controllers.Auth
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("api/v{version:apiVersion}/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(CancellationToken cancellationToken)
        {
            var query = new CurrentUserQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfile(
            [FromForm] UpdateProfileForm form,
            CancellationToken cancellationToken)
        {
            var command = new UpdateProfileCommand(form.ToDto());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var command = new LogoutCommand();
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
