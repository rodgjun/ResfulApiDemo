using ResfulApiDemo.Domain.Interfaces;
using ResfulApiDemo.Domain.Models;
using System.Text.Json;

namespace ResfulApiDemo.Data.Command
{
    public class ProductService : IProductServices
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            // Validate and assign HttpClient
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            // Set base address for all requests
            _httpClient.BaseAddress = new Uri("https://api.restful-api.dev/");
        }

        // Fetches a single product by ID from the API
        public async Task<Product> GetProductByIdAsync(string id)
        {
            // Validate input
            //if (string.IsNullOrEmpty(id)) throw new ArgumentException("ID cannot be null or empty", nameof(id));

            // Send GET request to /objects/{id}
            var response = await _httpClient.GetAsync($"objects/{id}");
            // Ensure the request succeeded
            response.EnsureSuccessStatusCode();

            // Read JSON response as string
            var json = await response.Content.ReadAsStringAsync();
            // Deserialize JSON into Product object
            var product = JsonSerializer.Deserialize<Product>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Handle case-insensitive property names
            });

            // Return product, or null if deserialization fails
            return product;
        }
    }
}

