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

        [HttpGet]
        public async Task<ActionResult<List<OrderResponse>>> GetAllAsync()
        {
            var orders = await _orderService.GetAllAsync();

            if (!orders.Any())
                return NotFound("No hay órdenes registradas.");

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateAsync([FromBody] OrderRequest order)
        {
            var createdOrder = await _orderService.CreateAsync(order);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] OrderRequest order, [FromRoute] Guid id)
        {
            await _orderService.UpdateAsync(order, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _orderService.DeleteAsync(id);
            return NoContent();
        }
    }
}
