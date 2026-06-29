using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetAllAsync();
        Task<OrderResponse> GetByIdAsync(Guid id);
        Task<OrderResponse> CreateAsync(OrderRequest order);
        Task UpdateAsync(OrderRequest order, Guid id);
        Task DeleteAsync(Guid id);
    }
}
