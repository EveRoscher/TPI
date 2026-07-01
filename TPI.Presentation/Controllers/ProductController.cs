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


        /// <summary>
        /// Obtiene todos los productos registrados en el menú.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<ProductResponse>>> GetAllAsync()
        {

            var products = await _productService.GetAllAsync();

            if (!products.Any())
                return NotFound("No hay productos registrados.");

            return Ok(products);
        }


        /// <summary>
        /// Obtiene un producto específico por su identificador único.
        /// </summary>
        /// <param name="id">Identificador único del producto.</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _productService.GetByIdAsync(id));
        }



        /// <summary>
        /// Crea un nuevo producto en el catálogo (Solo Admin).
        /// </summary>
        /// <param name="product">Datos del producto a crear.</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateAsync([FromBody] ProductRequest product)
        {
            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdProduct.Id }, createdProduct);
        }





        /// <summary>
        /// Elimina un producto por su identificador único (Solo Admin).
        /// </summary>
        /// <param name="id">Identificador único del producto a eliminar.</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Actualiza la información de un producto existente (Solo Admin).
        /// </summary>
        /// <param name="product">Nuevos datos del producto.</param>
        /// <param name="id">Identificador único del producto a actualizar.</param>
        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] ProductRequest product, [FromRoute] Guid id)
        {
            await _productService.UpdateAsync(product, id);
            return NoContent();
        }


    }
}
