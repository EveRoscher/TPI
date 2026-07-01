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

        /// <summary>
        /// Obtiene todos los pagos registrados (no rechazados/eliminados).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PaymentResponse>>> GetAllAsync()
        {
            var payments = await _paymentService.GetAllAsync();

            if (!payments.Any())
                return NotFound("No hay pagos registrados.");

            return Ok(payments);
        }

        /// <summary>
        /// Obtiene el listado de todos los pagos rechazados en el sistema (incluidos los eliminados lógicamente).
        /// </summary>
        [HttpGet("rejected")]
        public async Task<ActionResult<List<PaymentResponse>>> GetRejectedPaymentsAsync()
        {
            var payments = await _paymentService.GetRejectedPaymentsAsync();

            if (!payments.Any())
                return NotFound("No hay pagos rechazados registrados.");

            return Ok(payments);
        }

        /// <summary>
        /// Obtiene la información detallada de un pago por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del pago.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _paymentService.GetByIdAsync(id));
        }

        /// <summary>
        /// Registra un nuevo pago asociado a una orden y valida el monto.
        /// </summary>
        /// <param name="payment">Datos del pago (monto, orden asociada y método de pago).</param>
        [HttpPost]
        public async Task<ActionResult<PaymentResponse>> CreateAsync([FromBody] PaymentRequest payment)
        {
            var createdPayment = await _paymentService.CreateAsync(payment);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdPayment.Id }, createdPayment);
        }

        /// <summary>
        /// Actualiza un pago existente.
        /// </summary>
        /// <param name="payment">Nuevos datos del pago.</param>
        /// <param name="id">Identificador único del pago a actualizar.</param>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] PaymentRequest payment, [FromRoute] Guid id)
        {
            await _paymentService.UpdateAsync(payment, id);
            return NoContent();
        }

        /// <summary>
        /// Elimina lógicamente un pago.
        /// </summary>
        /// <param name="id">Identificador único del pago.</param>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _paymentService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Aprueba un pago, permitiendo que la orden asociada sea posteriormente entregada.
        /// </summary>
        /// <param name="id">Identificador único del pago a aprobar.</param>
        [HttpPost("{id}/accept")]
        public async Task<ActionResult> AcceptPaymentAsync([FromRoute] Guid id)
        {
            await _paymentService.AcceptPaymentAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Rechaza un pago, marcándolo como rechazado y eliminándolo (soft-delete) para permitir un nuevo intento de pago.
        /// </summary>
        /// <param name="id">Identificador único del pago a rechazar.</param>
        [HttpPost("{id}/reject")]
        public async Task<ActionResult> RejectPaymentAsync([FromRoute] Guid id)
        {
            await _paymentService.RejectPaymentAsync(id);
            return NoContent();
        }
    }
}
