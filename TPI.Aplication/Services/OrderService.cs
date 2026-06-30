using TPI.Aplication.Abstractions;
using TPI.Aplication.Abstractions.Infraestructure;
using TPI.Aplication.Exceptions;
using TPI.Aplication.Mappers;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Domain.Entities;

namespace TPI.Aplication.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDolarService _dolarService;
        private static decimal? _storedDolarRate;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IDolarService dolarService)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _dolarService = dolarService;
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

        public async Task<List<ProductResponse>> GetProductsByOrderIdAsync(Guid orderId)
        {
            var order = await _orderRepository.GetOrderWithItemsByIdAsync(orderId);

            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{orderId}'.");

            return order.OrderItems
                .Select(oi => oi.Product)
                .Where(p => p != null)
                .Select(p => p.ToProductResponse())
                .ToList();
        }

        public async Task RecalculateAllOrderTotalsAsync()
        {
            var orders = await _orderRepository.GetOrdersWithItemsAsync();
            foreach (var order in orders)
            {
                decimal total = order.OrderItems?.Sum(oi => oi.UnitPrice * oi.Quantity) ?? 0;
                order.TotalAmount = total;
                await _orderRepository.UpdateAsync(order);
            }
        }

        public async Task<OrderInfoResponse> GetOrderInfoByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderWithItemsByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{id}'.");

            return order.ToOrderInfoResponse();
        }

        public async Task CancelOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderWithItemsByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{id}'.");

            if (order.Status == OrderStatus.Canceled)
            {
                throw new ValidationException("La orden ya se encuentra cancelada.");
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new ValidationException("No se puede cancelar una orden que ya ha sido entregada.");
            }

            // Restore product stocks
            if (order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.Stock += item.Quantity;
                        await _productRepository.UpdateAsync(product);
                    }
                }
            }

            order.Status = OrderStatus.Canceled;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task SetOrderReadyAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{id}'.");

            if (order.Status == OrderStatus.Canceled)
            {
                throw new ValidationException("No se puede marcar como lista una orden cancelada.");
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new ValidationException("No se puede marcar como lista una orden entregada.");
            }

            order.Status = OrderStatus.Ready;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeliverOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetOrderWithPaymentByIdAsync(id);

            if (order == null)
                throw new NotFoundException($"No se encontró la orden con id '{id}'.");

            if (order.Status == OrderStatus.Canceled)
            {
                throw new ValidationException("No se puede entregar una orden que ha sido cancelada.");
            }

            if (order.Status == OrderStatus.Delivered)
            {
                throw new ValidationException("La orden ya se encuentra entregada.");
            }

            // Verify payment is accepted
            if (order.Payment == null || order.Payment.Status != PaymentStatus.Accepted || order.Payment.IsDeleted)
            {
                throw new ValidationException("No se puede entregar la orden porque no tiene un pago aprobado.");
            }

            order.Status = OrderStatus.Delivered;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task<decimal> FetchAndSaveDolarRateAsync()
        {
            var rate = await _dolarService.GetOfficialBuyRateAsync();
            _storedDolarRate = rate;
            return rate;
        }

        public async Task<OrderDolaresResponse> GetOrderInDollarsAsync(Guid id)
        {
            if (!_storedDolarRate.HasValue)
            {
                throw new ValidationException("No se ha obtenido la cotización del dólar. Ejecute el endpoint 'obtenerCotizacion' primero.");
            }

            var order = await _orderRepository.GetOrderWithItemsByIdAsync(id);
            if (order == null)
            {
                throw new NotFoundException($"No se encontró una orden con id '{id}'.");
            }

            var cotizacion = _storedDolarRate.Value;
            var response = new OrderDolaresResponse
            {
                Id = order.Id,
                Status = order.Status.ToString(),
                TotalAmountPesos = order.TotalAmount,
                TotalAmountDolares = Math.Round(order.TotalAmount / cotizacion, 2),
                CotizacionUsada = cotizacion,
                Products = new List<ProductDolaresResponse>()
            };

            foreach (var item in order.OrderItems)
            {
                if (item.Product != null)
                {
                    response.Products.Add(new ProductDolaresResponse
                    {
                        Name = item.Product.Name,
                        PrecioPesos = item.Product.Price,
                        PrecioDolares = Math.Round(item.Product.Price / cotizacion, 2),
                        Quantity = item.Quantity
                    });
                }
            }

            return response;
        }
    }
}
