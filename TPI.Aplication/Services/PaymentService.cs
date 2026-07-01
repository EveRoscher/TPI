using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
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
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{request.OrderId}'.");

            if (request.Amount != order.TotalAmount)
            {
                throw new ValidationException($"El monto del pago ({request.Amount}) debe ser igual al monto total de la orden ({order.TotalAmount}).");
            }

            if (request.Method == PaymentMethod.BankTransfer && request.Amount <= 100000)
            {
                throw new ValidationException("Las transferencias bancarias (BankTransfer) solo están permitidas para montos superiores a 100.000.");
            }

            if (request.Method == PaymentMethod.BankTransfer && string.IsNullOrWhiteSpace(request.ReceiptUrl))
            {
                throw new ValidationException("Las transferencias bancarias requieren el enlace al comprobante de pago.");
            }

            var existingPayments = await _paymentRepository.GetAllAsync();
            if (existingPayments.Any(p => p.OrderId == request.OrderId))
            {
                throw new ConflictException($"La orden con id '{request.OrderId}' ya tiene un pago registrado.");
            }

            var newPayment = request.ToPayment();
            await _paymentRepository.AddAsync(newPayment);
            return newPayment.ToPaymentResponse();
        }

        public async Task UpdateAsync(PaymentRequest request, Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            var order = await _orderRepository.GetByIdAsync(payment.OrderId);
            if (order == null)
                throw new NotFoundException($"No se encontró la orden asociada con id '{payment.OrderId}'.");

            if (request.Amount != order.TotalAmount)
            {
                throw new ValidationException($"El monto del pago ({request.Amount}) debe ser igual al monto total de la orden ({order.TotalAmount}).");
            }

            if (request.Method == PaymentMethod.BankTransfer && request.Amount <= 100000)
            {
                throw new ValidationException("Las transferencias bancarias (BankTransfer) solo están permitidas para montos superiores a 100.000.");
            }

            if (request.Method == PaymentMethod.BankTransfer && string.IsNullOrWhiteSpace(request.ReceiptUrl))
            {
                throw new ValidationException("Las transferencias bancarias requieren el enlace al comprobante de pago.");
            }

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

        public async Task AcceptPaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdWithDeletedAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            if (payment.IsDeleted || payment.Status == PaymentStatus.Rejected)
            {
                throw new ValidationException("No es posible aprobar un pago que ya ha sido rechazado o eliminado.");
            }

            if (payment.Status == PaymentStatus.Accepted)
            {
                throw new ValidationException("El pago ya se encuentra aprobado.");
            }

            payment.Status = PaymentStatus.Accepted;
            payment.ReceivedAt = DateTime.UtcNow;

            var order = await _orderRepository.GetByIdAsync(payment.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.Paid;
                await _orderRepository.UpdateAsync(order);
            }

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task RejectPaymentAsync(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
                throw new NotFoundException($"No se encontró un pago con id '{id}'.");

            payment.Status = PaymentStatus.Rejected;
            payment.IsDeleted = true;
            payment.DeletedDateTime = DateTime.UtcNow;
            payment.UpdatedDateTime = DateTime.UtcNow;

            var order = await _orderRepository.GetByIdAsync(payment.OrderId);
            if (order != null)
            {
                order.Status = OrderStatus.AwaitingPayment;
                await _orderRepository.UpdateAsync(order);
            }

            await _paymentRepository.UpdateAsync(payment);
        }

        public async Task<List<PaymentResponse>> GetRejectedPaymentsAsync()
        {
            var rejectedPayments = await _paymentRepository.GetRejectedPaymentsAsync();
            return rejectedPayments
                .Select(p => p.ToPaymentResponse())
                .ToList();
        }
    }
}
