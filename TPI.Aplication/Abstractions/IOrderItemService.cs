using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IOrderItemService
    {
        List<OrderItem> getAll();
        OrderItem getById(Guid id);
        OrderItem create(OrderItem orderItem);
        OrderItem update(OrderItem orderItem);
        OrderItem delete(OrderItem orderItem);
         object? GetAll();  

    }
}

