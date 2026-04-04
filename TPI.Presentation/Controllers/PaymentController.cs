using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService; //inyeccion de dependencias

        public PaymentController(IPaymentService paymentService) //creacion del constructor 
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Ok(_paymentService.GetAll());
        }
    }
}
