using Application.Features.Products.v1.Commands.ProductColor.CreateProductColor;
using Application.Features.Products.v1.Commands.ProductColor.DeleteProductColor;
using Application.Features.Products.v1.Commands.ProductColor.UpdateProductColor;
using Application.Features.Products.v1.Queries.ProductColors.GetColorsByProductId;
using Domain.Consts;

namespace WebAPI.Controllers.v1.Manager.Products
{
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = Roles.Manager)]
    [Route("api/v{version:apiVersion}/admin/product-colors")]
    public class ProductColorsManagerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductColorsManagerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductColor(CreateProductColorCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdateProductColor(int Id, UpdateProductColorCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> DeleteProductColor(int Id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductColorCommand(Id);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{productId:int}")]
        public async Task<IActionResult> GetColorsByProductId(int productId, CancellationToken cancellationToken)
        {
            var query = new GetColorsByProductIdQuery(productId);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
