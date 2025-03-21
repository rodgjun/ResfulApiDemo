using MediatR;                    // For IRequest
using ResfulApiDemo.Domain.Models;



namespace ResfulApiDemo.Services.ServiceQuery
{
    // Query to fetch all products from local DB
    public class GetAllProductsQuery : IRequest<ResultResponse>
    {
        // No parameters needed since it fetches all products
    }
}