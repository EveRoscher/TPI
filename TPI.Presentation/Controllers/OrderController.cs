using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Presentation.Authorization;

namespace TPI.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Obtiene todas las órdenes registradas en el sistema.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAllAsync()
        {
            var orders = await _orderService.GetAllAsync();

            if (!orders.Any())
                return NotFound("No hay órdenes registradas.");

            return Ok(orders);
        }

        /// <summary>
        /// Obtiene el detalle de una orden por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único (GUID) de la orden.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.GetByIdAsync(id));
        }

        /// <summary>
        /// Obtiene la información simplificada de cobro/pago de una orden.
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        [HttpGet("{id}/info")]
        public async Task<ActionResult<OrderInfoResponse>> GetOrderInfoByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.GetOrderInfoByIdAsync(id));
        }

        /// <summary>
        /// Obtiene el listado de productos incluidos en una orden específica.
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        [HttpGet("{id}/products")]
        public async Task<ActionResult<List<ProductResponse>>> GetProductsByOrderIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.GetProductsByOrderIdAsync(id));
        }

        /// <summary>
        /// Crea una nueva orden
        /// </summary>
        /// <param name="order">Datos de la orden.</param>
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateAsync([FromBody] OrderRequest order)
        {
            var createdOrder = await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdOrder.Id }, createdOrder);
        }

        /// <summary>
        /// Actualiza una orden existente.
        /// </summary>
        /// <param name="order">Nuevos datos de la orden.</param>
        /// <param name="id">Identificador único de la orden a actualizar.</param>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] OrderRequest order, [FromRoute] Guid id)
        {
            await _orderService.UpdateAsync(order, id);
            return NoContent();
        }

        /// <summary>
        /// Elimina lógicamente una orden del sistema.
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Cancela una orden activa y devuelve el stock reservado de los productos.
        /// </summary>
        /// <param name="id">Identificador único de la orden a cancelar.</param>
        [HttpPost("{id}/cancel")]
        public async Task<ActionResult> CancelOrderAsync([FromRoute] Guid id)
        {
            await _orderService.CancelOrderAsync(id);
            return Ok("La orden fue cancelada con éxito y el stock de los productos fue restablecido.");
        }

        /// <summary>
        /// Marca una orden en estado 'Ready' (Lista para retirar).
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        [HttpPost("{id}/ready")]
        public async Task<ActionResult> SetOrderReadyAsync([FromRoute] Guid id)
        {
            await _orderService.SetOrderReadyAsync(id);
            return Ok("La orden fue marcada como lista (Ready) con éxito.");
        }

        /// <summary>
        /// Marca la orden como 'Delivered' (Entregada). Requiere obligatoriamente un pago aprobado (Accepted).
        /// </summary>
        /// <param name="id">Identificador único de la orden a entregar.</param>
        [HttpPost("{id}/deliver")]
        public async Task<ActionResult> DeliverOrderAsync([FromRoute] Guid id)
        {
            await _orderService.DeliverOrderAsync(id);
            return Ok("La orden fue marcada como entregada (Delivered) con éxito.");
        }

        /// <summary>
        /// Recalcula e iguala los totales de todas las órdenes en base a los precios vigentes de sus productos.
        /// </summary>
        [HttpPost("recalculate-totals")]
        public async Task<ActionResult> RecalculateAllOrderTotalsAsync()
        {
            await _orderService.RecalculateAllOrderTotalsAsync();
            return Ok("Precios totales de todas las órdenes recalculados con éxito.");
        }

        /// <summary>
        /// Obtiene y guarda de forma temporal la cotización actual del dólar oficial.
        /// </summary>
        [HttpPost("obtenerCotizacion")]
        public async Task<ActionResult<decimal>> FetchAndSaveDolarRateAsync()
        {
            var rate = await _orderService.FetchAndSaveDolarRateAsync();
            return Ok(rate);
        }

        /// <summary>
        /// Obtiene la información simplificada de una orden y sus productos en dólares, usando la cotización guardada previamente.
        /// </summary>
        /// <param name="id">Identificador único de la orden.</param>
        [HttpGet("getOrderEnDolares/{id}")]
        public async Task<ActionResult<OrderDolaresResponse>> GetOrderEnDolaresAsync([FromRoute] Guid id)
        {
            var response = await _orderService.GetOrderInDollarsAsync(id);
            return Ok(response);
        }
    }
}
