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
        Task<List<ProductResponse>> GetProductsByOrderIdAsync(Guid orderId);
        Task RecalculateAllOrderTotalsAsync();
        Task<OrderInfoResponse> GetOrderInfoByIdAsync(Guid id);
        Task CancelOrderAsync(Guid id);
        Task SetOrderReadyAsync(Guid id);
        Task DeliverOrderAsync(Guid id);
        Task<decimal> FetchAndSaveDolarRateAsync();
        Task<OrderDolaresResponse> GetOrderInDollarsAsync(Guid id);
    }
}
