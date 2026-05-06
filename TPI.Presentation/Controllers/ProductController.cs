using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
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
        public ActionResult <ProductResponse> GetAll ()
        {
            
            return Ok(_productService.GetAll());

        }



        [HttpGet("{id}")]
        public ActionResult<ProductResponse> GetById(Guid id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }



        [HttpPost]
        public ActionResult<ProductResponse> Create(ProductRequests product)
        {
            var createdProduct = _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct); //nameof es el nombre del metodo que se va a llamar para obtener el producto creado
        }
}
}
