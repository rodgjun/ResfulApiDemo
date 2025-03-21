using MediatR;                    // For IRequest
using Restful.Demo.Domain.Models;

namespace Restful.Demo.Services.ServiceCommand
{
    // Command to update a product in local DB
    public class UpdateProductRequest : IRequest<ResultResponse>
    {
        public string Id { get; }      // ID of the product to update
        public string Name { get; }    // New name
        public Dictionary<string, object> Data { get; } // New data

        public UpdateProductRequest(string id, string name, Dictionary<string, object> data)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Data = data;
        }
    }
}