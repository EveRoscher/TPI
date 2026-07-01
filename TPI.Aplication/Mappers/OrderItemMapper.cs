using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItemResponse ToOrderItemResponse(this OrderItem orderItem)
        {
            return new OrderItemResponse
            {
                Id = orderItem.Id,
                NameSnapshot = orderItem.NameSnapshot,
                UnitPrice = orderItem.UnitPrice,
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                RemainingStock = orderItem.Product != null ? orderItem.Product.Stock : null,
                OrderTotal = orderItem.Order != null ? orderItem.Order.TotalAmount : null
            };
        }

        public static OrderItem ToOrderItem(this OrderItemRequest request, Product product)
        {
            return new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                ProductId = request.ProductId,
                NameSnapshot = product.Name,
                UnitPrice = product.Price,
                Quantity = request.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedDateTime = DateTime.UtcNow
            };
        }
    }
}
