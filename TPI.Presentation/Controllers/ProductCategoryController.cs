using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using TPI.Presentation.Authorization;

namespace TPI.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductCategoryResponse>>> GetAllAsync()
        {
            var categories = await _productCategoryService.GetAllAsync();

            if (!categories.Any())
                return NotFound("No hay categorías registradas.");

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductCategoryResponse>> GetByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _productCategoryService.GetByIdAsync(id));
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPost]
        public async Task<ActionResult<ProductCategoryResponse>> CreateAsync([FromBody] ProductCategoryRequest productCategory)
        {
            var created = await _productCategoryService.CreateAsync(productCategory);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync([FromBody] ProductCategoryRequest productCategory, [FromRoute] Guid id)
        {
            await _productCategoryService.UpdateAsync(productCategory, id);
            return NoContent();
        }

        [Authorize(Policy = Policies.SoloAdmin)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _productCategoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
