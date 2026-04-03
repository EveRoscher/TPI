using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Domain.Entities;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService; //inyeccion de dependencias

            
        public ProductController(IProductService productService) //creacion del constructor 
        {
            _productService = productService;
        }


        [HttpGet]
        public ActionResult <Product> GetAll ()
        {
            
            return Ok(_productService.GetAll());


        }
    }
}
