using Application.Features.Products.Commands.ProductImages.CreateProductImage;
using Application.Features.Products.Commands.ProductImages.DeleteProductImage;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage;
using Domain.Consts;
using WebAPI.Models.Products.Product;

namespace WebAPI.Controllers.Manager.Products
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Manager)]
    public class ProductImagesManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImagesManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImages([FromForm] UpsertProductImageForm form, CancellationToken cancellationToken)
        {
            var command = new CreateProductImagesCommand(form.CreateImages());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductImage([FromForm] UpsertProductImageForm form, CancellationToken cancellationToken)
        {
            var command = new UpdateProductImageCommand(form.UpdateImage());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProductImage(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductImageCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
