using Dapper;                     // For DB operations
using MediatR;                    // For IRequestHandler
using Microsoft.Data.SqlClient;   // For SqlConnection
using Restful.Demo.Services.ServiceCommand;    // For Commands
using System.Text.Json;
using System.Data;
using Restful.Demo.Domain.Models;
using Restful.Demo.Domain.Interfaces;           // For JSON serialization/deserialization
using Microsoft.Extensions.Configuration;
using Restful.Demo.Data.Command;
using Microsoft.IdentityModel.Tokens;

namespace Restful.Demo.Services.CommandHandler
{
    // Single handler class implementing multiple request handlers
    public class ProductCommandHandler :
    IRequestHandler<InsertProductRequest, ResultResponse>,
    IRequestHandler<UpdateProductRequest, ResultResponse>,
    IRequestHandler<DeleteProductRequest, ResultResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IProductServices _productService;

        public ProductCommandHandler(IProductRepository repository, IProductServices productService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        // Handle inserting a product from external API
        public async Task<ResultResponse> Handle(InsertProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product == null) throw new Exception($"Product with ID {request.Id} not found");
            await _repository.InsertAsync(product, cancellationToken);

            return new ResultResponse($"Succesfully Added object Id No.:{product.Id} to database", true, product);  // Return the fetched product
        }

        // Handle updating a product in local DB
        public async Task<ResultResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            if (request.Id.IsNullOrEmpty()) return new ResultResponse($"Product with ID {request.Id} not found");
            await _repository.UpdateAsync(request.Id, request.Name, request.Data, cancellationToken);
            return new ResultResponse("Successfully Updated!", true);
        }

        // Handle deleting a product from local DB
        public async Task<ResultResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id, cancellationToken);
            return new ResultResponse("Successfully Deleted!", true);
        }
    }
}