using Application.Features.Products.Commands.ProductVariants.CreateProductVariant;
using Application.Features.Products.Commands.ProductVariants.DeleteProductVariant;
using Application.Features.Products.Commands.ProductVariants.UpdateProductVariant;
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

        [HttpPut]
        public async Task<IActionResult> UpdateProductVariant(UpdateProductVariantCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProductVariant(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductVariantCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
