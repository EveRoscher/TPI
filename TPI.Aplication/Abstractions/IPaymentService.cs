using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions
{
    public interface IPaymentService
    {	List<Payment> getAll();
        Payment getById(Guid id);
        Payment create(Payment payment);
        Payment update(Payment payment);
        bool delete(Payment payment);
        object? GetAll();

    }
}

