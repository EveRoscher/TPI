using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Abstractions
{
    public interface IPaymentService
    {
        Task<List<PaymentResponse>> GetAllAsync();
        Task<PaymentResponse> GetByIdAsync(Guid id);
        Task<PaymentResponse> CreateAsync(PaymentRequest payment);
        Task UpdateAsync(PaymentRequest payment, Guid id);
        Task DeleteAsync(Guid id);
    }
}
