using MediatR;                    // For IRequest
using Restful.Demo.Domain.Models;

namespace Restful.Demo.Services.ServiceCommand
{
    // Command to delete a product from local DB
    public class DeleteProductRequest : IRequest<ResultResponse>
    {
        public string Id { get; set; }  // ID of the product to delete

        //public DeleteProductRequest(string id)
        //{
        //    Id = id ?? throw new ArgumentNullException(nameof(id));
        //}
    }
}