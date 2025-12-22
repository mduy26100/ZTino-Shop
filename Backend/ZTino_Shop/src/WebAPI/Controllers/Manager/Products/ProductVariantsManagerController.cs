using Application.Features.Products.Commands.ProductVariants.CreateProductVariant;
using Domain.Consts;

namespace WebAPI.Controllers.Manager.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Manager)]
    public class ProductVariantsManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductVariantsManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductVariant(CreateProductVariantCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
