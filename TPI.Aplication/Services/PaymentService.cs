using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<PaymentResponse>> GetAllAsync()
        {
            return (await _paymentRepository.GetAllAsync())
                .OrderByDescending(x => x.ReceivedAt)
                .Select(x => x.ToPaymentResponse())
                .ToList();
        }

        public async Task<PaymentResponse> GetByIdAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            return payment.ToPaymentResponse();
        }

        public async Task<PaymentResponse> CreateAsync(PaymentRequest request)
        {
            var newPayment = request.ToPayment();
            await _paymentRepository.AddAsync(newPayment);
            return newPayment.ToPaymentResponse();
        }

        public async Task UpdateAsync(PaymentRequest request, Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            payment.Method = request.Method;
            payment.Amount = request.Amount;
            payment.ReceiptUrl = request.ReceiptUrl;

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task DeleteAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            await _paymentRepository.DeleteAsync(id);
        }
    }
}
