using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Domain.Entities;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService; //inyeccion de dependencias

        public OrderItemController (IOrderItemService orderItemService) //creacion del constructor 
        {
            _orderItemService = orderItemService;
        }

        [HttpGet]
        public ActionResult<OrderItem> GetAll()
        {
            var orderItems = _orderItemService.GetAll();
            return Ok(orderItems);
        }
    }
}
