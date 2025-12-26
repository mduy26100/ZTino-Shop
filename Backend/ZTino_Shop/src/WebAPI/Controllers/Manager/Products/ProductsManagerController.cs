using Application.Features.Products.Commands.Products.CreateProduct;
using Application.Features.Products.Commands.Products.DeleteProduct;
using Application.Features.Products.Commands.Products.UpdateProduct;
using Domain.Consts;
using WebAPI.Models.Products.Product;

namespace WebAPI.Controllers.Manager.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Manager)]
    public class ProductsManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProduct(
            [FromForm] UpsertProductForm form,
            CancellationToken cancellationToken)
        {
            var command = new CreateProductCommand(form.ToDto());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProduct(int Id,
            [FromForm] UpsertProductForm form,
            CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(form.ToDto());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProduct(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
