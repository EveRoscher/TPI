using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<List<OrderResponse>> GetAllAsync()
        {
            return (await _orderRepository.GetAllAsync())
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.ToOrderResponse())
                .ToList();
        }

        public async Task<OrderResponse> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró una orden con id '{id}'.");

            return order.ToOrderResponse();
        }

        public async Task<OrderResponse> CreateAsync(OrderRequest request)
        {
            var newOrder = request.ToOrder();
            await _orderRepository.AddAsync(newOrder);
            return newOrder.ToOrderResponse();
        }

        public async Task UpdateAsync(OrderRequest request, Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró una orden con id '{id}'.");

            order.PickupETA = request.PickupETA;
            order.PickupDay = request.PickupDay;
            order.UserId = request.UserId;

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró una orden con id '{id}'.");

            await _orderRepository.DeleteAsync(id);
        }
    }
}
