using Restful.Demo.Domain.Models;

namespace Restful.Demo.Domain.Interfaces
{
    public interface IProductServices
    {
        Task<Product> GetProductByIdAsync(string id);
    }
}
