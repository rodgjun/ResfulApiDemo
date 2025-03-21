using MediatR;                    // For IRequest
using Restful.Demo.Domain.Models;

namespace Restful.Demo.Services.ServiceCommand
{
    // Command to insert a product by fetching from external API
    public class InsertProductRequest : IRequest<ResultResponse>
    {
        public string Id { get; }  // ID of the product to fetch and insert

        public InsertProductRequest(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}