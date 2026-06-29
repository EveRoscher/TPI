using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class PaymentMapper
    {
        public static PaymentResponse ToPaymentResponse(this Payment payment)
        {
            return new PaymentResponse
            {
                Id = payment.Id,
                Method = payment.Method.ToString(),
                Amount = payment.Amount,
                Status = payment.Status.ToString(),
                ReceiptUrl = payment.ReceiptUrl,
                ReceivedAt = payment.ReceivedAt,
                OrderId = payment.OrderId
            };
        }

        public static Payment ToPayment(this PaymentRequest request)
        {
            return new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                Method = request.Method,
                Amount = request.Amount,
                ReceiptUrl = request.ReceiptUrl,
                Status = PaymentStatus.PendingReview,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
