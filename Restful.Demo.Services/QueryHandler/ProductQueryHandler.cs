using Dapper;                     // For DB operations
using MediatR;                    // For IRequestHandler
using Microsoft.Data.SqlClient;   // For SqlConnection
using Restful.Demo.Services.ServiceQuery;     // For Queries
using System.Text.Json;
using System.Data;
using Restful.Demo.Domain.Models;
using Restful.Demo.Domain.Interfaces;
using Microsoft.Extensions.Configuration;           // For JSON serialization/deserialization

namespace Restful.Demo.Services.QueryHandler
{
    // Single handler class implementing multiple request handlers
    public class ProductQueryHandler :
    IRequestHandler<GetProductQuery, ResultResponse>,
        IRequestHandler<GetAllProductsQuery, ResultResponse>
    {
        //private readonly string _connectionString;         // DB connection string
        private readonly IProductRepository _repository;
        private readonly IProductServices _productService;

        public ProductQueryHandler(IProductRepository repository, IProductServices productService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        // Handle fetching a product by ID from local DB
        public async Task<ResultResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(request.Id, cancellationToken);
        }

        // Handle fetching all products from local DB
        public async Task<ResultResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }
    }
}