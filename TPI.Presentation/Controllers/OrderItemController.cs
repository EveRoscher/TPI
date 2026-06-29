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
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderItemResponse>>> GetAllAsync()
        {
            var items = await _orderItemService.GetAllAsync();

            if (!items.Any())
                return NotFound("No hay items registrados.");

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderItemResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _orderItemService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<OrderItemResponse>> CreateAsync([FromBody] OrderItemRequest orderItem)
        {
            var createdItem = await _orderItemService.CreateAsync(orderItem);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdItem.Id }, createdItem);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] OrderItemRequest orderItem, [FromRoute] Guid id)
        {
            await _orderItemService.UpdateAsync(orderItem, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _orderItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
