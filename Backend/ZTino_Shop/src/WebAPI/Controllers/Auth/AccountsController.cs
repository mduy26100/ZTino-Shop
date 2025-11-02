using Application.Features.Auth.Commands.UpdateProfile;
using Application.Features.Auth.Queries.CurrentUser;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Models.Auth;

namespace WebAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
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
    }
}
