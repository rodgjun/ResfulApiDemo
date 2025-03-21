using MediatR;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ResfulApiDemo.Domain.Interfaces;
using ResfulApiDemo.Domain.Models;
using ResfulApiDemo.Services.ServiceQuery;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ResfulApiDemo.Services.QueryHandler
{
    public class ProductQueryHandler
    {
        // Query Handlers
        public class GetProductHandler : IRequestHandler<GetProductQuery, ResultResponse>
        {
            private readonly string _connectionString;

            public GetProductHandler(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException(nameof(configuration));
            }

            public async Task<ResultResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var result = await connection.QuerySingleOrDefaultAsync<dynamic>(
                        "usp_GetProductById",
                        new { request.Id },
                        commandType: CommandType.StoredProcedure
                    );

                    if (result == null) return new ResultResponse($"Error: {new ArgumentException(nameof(result))}, Id cannot be found");

                    var product = new Product
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Data = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Data)
                    };

                    return new ResultResponse($"Id No. {result.Id} succesfully found", true, product);
                }
            }
        }

        public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, ResultResponse>
        {
            private readonly string _connectionString;

            public GetAllProductsHandler(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException(nameof(configuration));
            }

            public async Task<ResultResponse> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    var results = await connection.QueryAsync<dynamic>(
                        "usp_GetProduct",
                        commandType: CommandType.StoredProcedure
                    );

                    var products = new List<Product>();
                    foreach (var result in results)
                    {
                        products.Add(new Product
                        {
                            Id = result.Id,
                            Name = result.Name,
                            Data = string.IsNullOrEmpty(result.Data)
                                ? null
                                : JsonSerializer.Deserialize<Dictionary<string, object>>(result.Data)
                        });
                    }

                    return new ResultResponse("Successfully retrieved products", true, products);
                }
            }
        }
    }
}
