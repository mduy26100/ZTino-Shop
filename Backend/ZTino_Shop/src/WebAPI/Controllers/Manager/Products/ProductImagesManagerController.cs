using Application.Features.Products.Commands.ProductImages.CreateProductImage;
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
        public async Task<IActionResult> CreateProductImage([FromForm] UpsertProductImageForm form, CancellationToken cancellationToken)
        {
            var command = new CreateProductImagesCommand(form.CreateImages());
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
