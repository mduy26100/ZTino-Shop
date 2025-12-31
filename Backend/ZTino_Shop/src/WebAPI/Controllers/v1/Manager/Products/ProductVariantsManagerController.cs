using Application.Features.Products.v1.Commands.ProductVariants.DeleteProductVariant;
using Application.Features.Products.v1.Commands.ProductVariants.CreateProductVariant;
using Application.Features.Products.v1.Commands.ProductVariants.UpdateProductVariant;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/product-variants")]
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

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateProductVariant(int Id, UpdateProductVariantCommand command, CancellationToken cancellationToken)
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
