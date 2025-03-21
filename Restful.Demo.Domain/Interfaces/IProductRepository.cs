using Restful.Demo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restful.Demo.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<ResultResponse> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<ResultResponse> GetAllAsync(CancellationToken cancellationToken);
        Task<ResultResponse> InsertAsync(Product product, CancellationToken cancellationToken);
        Task<ResultResponse> UpdateAsync(string id, string name, Dictionary<string, object> data, CancellationToken cancellationToken);
        Task<ResultResponse> DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
