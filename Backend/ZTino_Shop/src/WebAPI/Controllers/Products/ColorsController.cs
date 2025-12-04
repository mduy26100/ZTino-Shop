using Application.Features.Products.Queries.Colors.GetAllColors;

namespace WebAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
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
