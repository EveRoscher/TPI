using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Aplication.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IProductRepository _productRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IProductRepository productRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
        }

        public async Task<List<OrderItemResponse>> GetAllAsync()
        {
            return (await _orderItemRepository.GetAllAsync())
                .Select(x => x.ToOrderItemResponse())
                .ToList();
        }

        public async Task<OrderItemResponse> GetByIdAsync(Guid id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"No se encontró el item con id '{id}'.");

            return item.ToOrderItemResponse();
        }

        public async Task<OrderItemResponse> CreateAsync(OrderItemRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
                throw new NotFoundException($"No se encontró un producto con id '{request.ProductId}'.");

            var newItem = request.ToOrderItem(product);
            await _orderItemRepository.AddAsync(newItem);
            return newItem.ToOrderItemResponse();
        }

        public async Task UpdateAsync(OrderItemRequest request, Guid id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"No se encontró el item con id '{id}'.");

            item.Quantity = request.Quantity;

            await _orderItemRepository.UpdateAsync(item);
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"No se encontró el item con id '{id}'.");

            await _orderItemRepository.DeleteAsync(id);
        }
    }
}
