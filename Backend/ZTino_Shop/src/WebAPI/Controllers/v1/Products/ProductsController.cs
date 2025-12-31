using Application.Features.Products.v1.Queries.Products.GetAllProducts;
using Application.Features.Products.v1.Queries.Products.GetProductDetail;

namespace WebAPI.Controllers.v1.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductDetail(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductDetailQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
