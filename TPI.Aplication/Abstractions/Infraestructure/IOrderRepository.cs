using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions.Infraestructure
{
    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<Order?> GetOrderWithItemsByIdAsync(Guid id);
        Task<List<Order>> GetOrdersWithItemsAsync();
        Task<Order?> GetOrderWithPaymentByIdAsync(Guid id);
    }
}
