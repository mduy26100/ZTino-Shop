using Application.Features.Products.Commands.ProductImages.CreateProductImage;
using Application.Features.Products.Commands.ProductImages.DeleteProductImage;
using Application.Features.Products.Commands.ProductImages.UpdateProductImage;
using Application.Features.Products.Queries.ProductImages.GetProductImagesByProductVariantId;
using Domain.Consts;
using WebAPI.Requests.Products.Product;

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

        [HttpGet("{variantId:int}/images")]
        public async Task<IActionResult> GetProductImagesByProductVariantId(int variantId, CancellationToken cancellationToken)
        {
            var query = new GetProductImagesByProductVariantIdQuery(variantId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateProductImages([FromForm] UpsertProductImageForm form, CancellationToken cancellationToken)
        {
            var command = new CreateProductImagesCommand(form.CreateImages());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{Id:int}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProductImage(int Id, [FromForm] UpsertProductImageForm form, CancellationToken cancellationToken)
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
