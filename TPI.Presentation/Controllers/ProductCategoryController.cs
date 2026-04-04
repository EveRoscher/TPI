using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Aplication.Services;
using TPI.Domain.Entities;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService; //inyeccion de dependencias

        public ProductCategoryController(IProductCategoryService productCategoryService) //creacion del constructor 
        {
            _productCategoryService = productCategoryService;
        }

        [HttpGet]
        public ActionResult<ProductCategory> GetAll()
        {

            return Ok(_productCategoryService.GetAll());

        }

    }
}
