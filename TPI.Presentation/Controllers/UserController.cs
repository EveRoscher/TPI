using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Domain.Entities;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase  
    {
        private readonly IUserService _userService; //inyeccion de dependencias

        public UserController(IUserService userService) //creacion del constructor 
        {
            _userService = userService;
        }


        [HttpGet]
                public ActionResult<User> GetAll()
                {
                    return Ok(_userService.GetAll());
                }

    }
}




   