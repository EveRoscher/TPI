using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IOrderItemService
    {
        Task<List<OrderItemResponse>> GetAllAsync();
        Task<OrderItemResponse> GetByIdAsync(Guid id);
        Task<OrderItemResponse> CreateAsync(OrderItemRequest orderItem);
        Task UpdateAsync(OrderItemRequest orderItem, Guid id);
        Task DeleteAsync(Guid id);
    }
}
