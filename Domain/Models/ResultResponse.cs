namespace ResfulApiDemo.Domain.Models
{
    // Response model for CRUD operations to indicate success or failure
    public class ResultResponse
    {
        // Indicates whether the operation was successful
        public bool Success { get; set; }

        // Stores any error message if the operation fails
        public string Exception { get; set; }
        public string Response { get; set; }
        public Product Product { get; set; }
        public List<Product> Products { get; set; }

        // Constructor for fail
        public ResultResponse(string exception)
        {
            Success = false;
            Exception = exception;
        }
        // Constructor for success
        public ResultResponse(string response, bool success)
        {
            Success = success;
            Response = response;
        }
        // Constructor for success
        public ResultResponse(string response, bool success, Product product)
        {
            Success = success;
            Response = response;
            Product = product;
        }
        public ResultResponse(string response, bool success, List<Product> products)
        {
            Success = success;
            Response = response;
            Products = products;
        }
    }
}