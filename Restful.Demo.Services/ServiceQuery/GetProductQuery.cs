using MediatR;                    // For IRequest
using Restful.Demo.Domain.Models;

namespace Restful.Demo.Services.ServiceQuery
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