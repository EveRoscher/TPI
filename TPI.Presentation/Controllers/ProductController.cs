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
        public ActionResult<ProductResponse> GetAll()
        {
            var products = _productService.GetAll();

            if (!products.Any())
                return NotFound();

            return Ok(products);
        }



        [HttpGet("{id}")]
        public ActionResult<ProductResponse> GetById([FromRoute] Guid id)
        {
            var product = _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }



        [HttpPost]
        public ActionResult<ProductResponse> Create([FromBody] ProductRequests product)
        {
            var createdProduct = _productService.Create(product);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct); //nameof es el nombre del metodo que se va a llamar para obtener el producto creado
        }





        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id) 
        {
            var deleted = _productService.Delete(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromBody] ProductRequests product, [FromRoute] Guid id)
        
        {
            var updatedProduct = _productService.Update(product, id);

            if (!updatedProduct)
                return NotFound();

            return NoContent();
        }


    }
}
