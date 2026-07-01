using TPI.Domain.Entities;

namespace TPI.Aplication.Abstractions.Infraestructure
{
    public interface IPaymentRepository : IBaseRepository<Payment>
    {
        Task<List<Payment>> GetRejectedPaymentsAsync();
        Task<Payment?> GetByIdWithDeletedAsync(Guid id);
    }
}
