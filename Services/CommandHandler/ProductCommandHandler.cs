using Dapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using ResfulApiDemo.Domain.Interfaces;
using ResfulApiDemo.Domain.Models;
using ResfulApiDemo.Services.ServiceCommand;
using ResfulApiDemo.Services.ServiceQuery;
using System.Data;
using System.Text.Json;

namespace ResfulApiDemo.Services.CommandHandler
{
    // Command Handlers
    public class InsertProductHandler : IRequestHandler<InsertProductRequest, ResultResponse>
    {
        private readonly IProductServices _productService;
        private readonly string _connectionString;

        public InsertProductHandler(IProductServices productService, IConfiguration configuration)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ResultResponse> Handle(InsertProductRequest request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetProductByIdAsync(request.Id);
            if (product == null) new ResultResponse($"Product with ID {request.Id} not found", false);

            var dataJson = JsonSerializer.Serialize(product?.Data);

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync(
                    "usp_InsertProduct",
                    new { product.Id, product.Name, Data = dataJson },
                    commandType: CommandType.StoredProcedure
                );
            }

            return new ResultResponse($"Succesfully Added object Id No.:{product.Id} to database", true, product);
        }
    }

    public class UpdateProductHandler : IRequestHandler<UpdateProductRequest, ResultResponse>
    {
        private readonly string _connectionString;

        public UpdateProductHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ResultResponse> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dataJson = JsonSerializer.Serialize(request.Data);

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    await connection.ExecuteAsync(
                        "usp_UpdateProduct",
                        new { request.Id, request.Name, Data = dataJson },
                        commandType: CommandType.StoredProcedure
                    );

                    return new ResultResponse("Successfully Updated", true);
                }
            }
            catch (Exception ex)
            {
                return new ResultResponse(ex.Message, false);
            }
        }
    }

    public class DeleteProductHandler : IRequestHandler<DeleteProductRequest, ResultResponse>
    {
        private readonly string _connectionString;

        public DeleteProductHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<ResultResponse> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    await connection.ExecuteAsync(
                        "usp_DeleteProduct",
                        new { request.Id },
                        commandType: CommandType.StoredProcedure
                    );

                    return new ResultResponse("Successfully Deleted", true);
                }
            }
            catch (Exception ex)
            {
                return new ResultResponse($"Error: {ex.Message}");
            }
        }
    }
}