using Application.Features.Products.v1.Commands.ProductImages.CreateProductImages;
using Application.Features.Products.v1.Commands.ProductImages.DeleteProductImage;
using Application.Features.Products.v1.Commands.ProductImages.UpdateProductImage;
using Application.Features.Products.v1.Queries.ProductImages.GetProductImagesByProductColorId;
using Domain.Consts;
using WebAPI.Requests.Products.Product;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/product-images")]
    public class ProductImagesManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductImagesManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductImagesByProductColorId([FromQuery] int productColorId, CancellationToken cancellationToken)
        {
            var query = new GetProductImagesByProductColorIdQuery(productColorId);
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
