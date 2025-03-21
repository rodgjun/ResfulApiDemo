using MediatR;                    // For IRequest
using ResfulApiDemo.Domain.Models;

namespace ResfulApiDemo.Services.ServiceQuery
{
    // Query to fetch and store a product by ID
    public class GetProductQuery : IRequest<ResultResponse>
    {
        // The ID of the product to fetch
        public string Id { get; set; }

        // Constructor to set the ID
        //public GetProductQuery(string id)
        //{
        //    Id = id ?? throw new ArgumentNullException(nameof(id));
        //}
    }
}