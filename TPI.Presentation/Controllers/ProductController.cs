using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Exceptions;
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
            try
            {
                var products = _productService.GetAll();

                if (!products.Any())
                    return NotFound("No hay productos registrados.");

                return Ok(products);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }



        [HttpGet("{id}")]
        public ActionResult<ProductResponse> GetById([FromRoute] Guid id)
        {
            try
            {
                return Ok(_productService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }



        [HttpPost]
        public ActionResult<ProductResponse> Create([FromBody] ProductRequest product)
        {
            try
            {
                var createdProduct = _productService.Create(product);
                return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }





        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] Guid id) 
        {
            try
            {
                _productService.Delete(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }


        [HttpPut("{id}")]
        public ActionResult Update([FromBody] ProductRequest product, [FromRoute] Guid id)
        
        {
            try
            {
                _productService.Update(product, id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DatabaseException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }


    }
}
