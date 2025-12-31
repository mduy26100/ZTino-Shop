using Application.Features.Products.v1.Queries.Colors.GetAllColors;

namespace WebAPI.Controllers.v1.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/colors")]
    public class ColorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ColorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllColors()
        {
            var query = new GetAllColorsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
