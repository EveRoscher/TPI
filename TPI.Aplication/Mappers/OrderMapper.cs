using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class OrderMapper
    {
        public static OrderResponse ToOrderResponse(this Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalAmount = order.TotalAmount,
                PickupETA = order.PickupETA,
                PickupDay = order.PickupDay,
                CreatedAt = order.CreatedAt,
                ConfirmedAt = order.ConfirmedAt,
                UserId = order.UserId
            };
        }

        public static Order ToOrder(this OrderRequest request)
        {
            return new Order
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                PickupETA = request.PickupETA,
                PickupDay = request.PickupDay,
                Status = OrderStatus.Created,
                TotalAmount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
