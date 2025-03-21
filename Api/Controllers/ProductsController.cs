using MediatR;                    // For IMediator
using Microsoft.AspNetCore.Mvc;   // For MVC features
using Microsoft.IdentityModel.Tokens;
using Restful.Demo.Domain.Models;
using Restful.Demo.Services.ServiceCommand;
using Restful.Demo.Services.ServiceQuery;

namespace Restful.Demo.Api.Controllers 
{
    [ApiController]
    [Route("products/")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;  // Mediator for CQRS

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // POST: Insert product by fetching from external API
        [HttpPost("insert")]
        public async Task<ActionResult<Product>> InsertProduct([FromBody] InsertProductRequest command)
        {
            // Validate ID
            if (string.IsNullOrWhiteSpace(command.Id))
                return BadRequest("Product ID cannot be empty.");

            var product = await _mediator.Send(command);  // Dispatch to handler
            return Ok(product);                           // Return product
        }

        // GET: Read product from local DB by ID
        [HttpGet("read")]
        public async Task<ActionResult<ResultResponse>> GetProduct([FromBody] GetProductQuery query)
        {
            // Validate ID
            if (query.Id.IsNullOrEmpty())
                return BadRequest("Product ID cannot be empty.");

            return await _mediator.Send(query);
        }

        // GET: Read all products from local DB
        [HttpGet("all")]
        public async Task<ActionResult<ResultResponse>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();      // Create query
            return await _mediator.Send(query); // Dispatch to handler
         }

        // POST: Update product in local DB
        [HttpPost("update")]
        public async Task<ResultResponse> UpdateProduct([FromBody] UpdateProductRequest command)
        {
            // Validate inputs
            if (command.Id.IsNullOrEmpty())
                return new ResultResponse("Product ID cannot be empty.", false);

            return await _mediator.Send(command);  // Dispatch to handler
        }

        // POST: Delete product from local DB
        [HttpPost("delete")]
        public async Task<ResultResponse> DeleteProduct([FromBody] DeleteProductRequest command)
        {
            // Validate ID
            if (command.Id.IsNullOrEmpty())  // Fixed from IsNullOrEmpty
                return new ResultResponse("Product ID cannot be empty.", false);

            return await _mediator.Send(command);  // Dispatch to handler
        }
    }
}