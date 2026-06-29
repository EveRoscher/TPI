using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Presentation.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TPI.Presentation.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<List<ProductResponse>>> GetAllAsync()
        {

            var products = await _productService.GetAllAsync();

            if (!products.Any())
                return NotFound("No hay productos registrados.");

            return Ok(products);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }



        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateAsync([FromBody] ProductRequest product)
        {
            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdProduct.Id }, createdProduct);
        }





        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] ProductRequest product, [FromRoute] Guid id)
        {
            await _productService.UpdateAsync(product, id);
            return NoContent();
        }


    }
}
