using Application.Features.Products.v1.Queries.Sizes.GetAllSizes;

namespace WebAPI.Controllers.v1.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/sizes")]
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
