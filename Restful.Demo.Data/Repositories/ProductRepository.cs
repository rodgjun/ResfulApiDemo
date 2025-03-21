using Microsoft.Data.SqlClient;
using Restful.Demo.Domain.Interfaces;
using Restful.Demo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dapper;
using Azure.Core;
using MediatR;
using Restful.Demo.Data.Command;

namespace Restful.Demo.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        private readonly IProductServices _productService;

        public ProductRepository(IConfiguration configuration, IProductServices productService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
            _productService = productService;
        }

        public async Task<ResultResponse> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);  // Open DB connection
                var result = await connection.QuerySingleOrDefaultAsync<dynamic>(  // Fetch dynamically
                    "usp_GetProductById",
                    new { Id = id },
                    commandType: CommandType.StoredProcedure
                );

                if (result == null) return new ResultResponse($"Error: {new ArgumentException(nameof(result))}, Id cannot be found");  // Return null if not found

                var product = new Product  // Map to Product model
                {
                    Id = result.Id,
                    Name = result.Name,
                    Data = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Data)
                };

                return new ResultResponse($"Id No. {result.Id} succesfully found", true, product);
            }
        }

        public async Task<ResultResponse> GetAllAsync(CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);  // Open DB connection
                var results = await connection.QueryAsync<dynamic>(  // Fetch all dynamically
                    "usp_GetProduct",
                    commandType: CommandType.StoredProcedure
                );

                var products = new List<Product>();  // Initialize result list
                foreach (var result in results)
                {
                    products.Add(new Product  // Map each result to Product
                    {
                        Id = result.Id,
                        Name = result.Name,
                        Data = string.IsNullOrEmpty(result.Data)
                            ? null
                            : JsonSerializer.Deserialize<Dictionary<string, object>>(result.Data)
                    });
                }

                return new ResultResponse("Successfully retrieved products", true, products);  // Return success with product list
            }
        }

        public async Task<ResultResponse> InsertAsync(Product product, CancellationToken cancellationToken)
        {
            var dataJson = JsonSerializer.Serialize(product?.Data);  // Serialize Data for DB

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(
                "usp_InsertProduct",
                new { Id = product.Id, Name = product.Name, Data = dataJson },
                commandType: CommandType.StoredProcedure);

            return new ResultResponse($"Succesfully Added object Id No.:{product.Id} to database", true, product);  // Return the fetched product 
        }

        public async Task<ResultResponse> UpdateAsync(string id, string name, Dictionary<string, object> data, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(
                "usp_UpdateProduct",
                new { Id = id, Name = name, Data = JsonSerializer.Serialize(data) },
                commandType: CommandType.StoredProcedure);

            return new ResultResponse("Successfully Updated!", true);
        }

        public async Task<ResultResponse> DeleteAsync(string id, CancellationToken cancellationToken)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            await connection.ExecuteAsync(
                "usp_DeleteProduct",
                new { Id = id },
                commandType: CommandType.StoredProcedure);

            return new ResultResponse("Successfully Deleted!", true);
        }
    }
}
