using MediatR;                    // For IRequest
using Restful.Demo.Domain.Models;

namespace Restful.Demo.Services.ServiceQuery
{
    // Query to fetch all products from local DB
    public class GetAllProductsQuery : IRequest<ResultResponse>
    {
        // No parameters needed since it fetches all products
    }
}