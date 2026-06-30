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
        private readonly IOrderRepository _orderRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _orderItemRepository = orderItemRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
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

            if (!product.IsActive)
                throw new ValidationException($"El producto '{product.Name}' no está activo y no se puede agregar a la orden.");

            if (product.Stock < request.Quantity)
                throw new ValidationException($"Stock insuficiente para el producto '{product.Name}'. Stock disponible: {product.Stock}, solicitado: {request.Quantity}.");

            product.Stock -= request.Quantity;
            await _productRepository.UpdateAsync(product);
    
            var newItem = request.ToOrderItem(product);
            await _orderItemRepository.AddAsync(newItem);

            decimal newOrderTotal = 0;
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order != null)
            {
                order.TotalAmount += newItem.UnitPrice * newItem.Quantity;
                await _orderRepository.UpdateAsync(order);
                newOrderTotal = order.TotalAmount;
            }
            
            var response = newItem.ToOrderItemResponse();
            response.RemainingStock = product.Stock;
            response.OrderTotal = newOrderTotal;
            return response;
        }

        public async Task UpdateAsync(OrderItemRequest request, Guid id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"No se encontró el item con id '{id}'.");

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new NotFoundException($"No se encontró el producto asociado con id '{item.ProductId}'.");

            int difference = request.Quantity - item.Quantity;
            if (difference > 0)
            {
                if (product.Stock < difference)
                    throw new ValidationException($"Stock insuficiente para el producto '{product.Name}'. Stock disponible adicional: {product.Stock}, requerido adicional: {difference}.");
            }

            product.Stock -= difference;
            await _productRepository.UpdateAsync(product);

            decimal oldSubtotal = item.UnitPrice * item.Quantity;
            item.Quantity = request.Quantity;
            await _orderItemRepository.UpdateAsync(item);

            var order = await _orderRepository.GetByIdAsync(item.OrderId);
            if (order != null)
            {
                order.TotalAmount += (item.UnitPrice * request.Quantity) - oldSubtotal;
                await _orderRepository.UpdateAsync(order);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
                throw new NotFoundException($"No se encontró el item con id '{id}'.");

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            var order = await _orderRepository.GetByIdAsync(item.OrderId);
            if (order != null)
            {
                order.TotalAmount -= item.UnitPrice * item.Quantity;
                if (order.TotalAmount < 0) order.TotalAmount = 0;
                await _orderRepository.UpdateAsync(order);
            }

            await _orderItemRepository.DeleteAsync(id);
        }
    }
}
