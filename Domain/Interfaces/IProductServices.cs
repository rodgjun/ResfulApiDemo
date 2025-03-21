using ResfulApiDemo.Domain.Models;

namespace ResfulApiDemo.Domain.Interfaces
{
    public interface IProductServices
    {
        Task<Product> GetProductByIdAsync(string id);
    }
}
