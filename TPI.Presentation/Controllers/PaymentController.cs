using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;

namespace TPI.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PaymentResponse>>> GetAllAsync()
        {
            var payments = await _paymentService.GetAllAsync();

            if (!payments.Any())
                return NotFound("No hay pagos registrados.");

            return Ok(payments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _paymentService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> CreateAsync([FromBody] PaymentRequest payment)
        {
            var createdPayment = await _paymentService.CreateAsync(payment);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdPayment.Id }, createdPayment);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] PaymentRequest payment, [FromRoute] Guid id)
        {
            await _paymentService.UpdateAsync(payment, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _paymentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
