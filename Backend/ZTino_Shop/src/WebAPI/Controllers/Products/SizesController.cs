using Application.Features.Products.Queries.Sizes.GetAllSizes;

namespace WebAPI.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SizesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSizes(CancellationToken cancellationToken)
        {
            var query = new GetAllSizesQuery();
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
