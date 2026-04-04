using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IOrderService
    {
        List<Order> getAll();
        Order getById(Guid id);
        Order create(Order order);
        Order update(Order order);
        Order delete(Order order);
        object? GetAll();

    }
}

