using TPI.Aplication.Abstractions;
using TPI.Aplication.Requests;
using TPI.Aplication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace TPI.Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registra a un nuevo usuario (Cliente) en el sistema.
        /// </summary>
        /// <param name="request">Datos del registro (nombre, email, contraseña).</param>
        [HttpPost("signup")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> SignUp([FromBody] SignUpRequest request)
        {
            var response = _authService.SignUp(request);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Inicia sesión y devuelve el token JWT del usuario registrado.
        /// </summary>
        /// <param name="request">Credenciales del usuario (email y contraseña).</param>
        [HttpPost("signin")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> SignIn([FromBody] SignInRequest request)
        {
            return Ok(_authService.SignIn(request));
        }
    }
}

