using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TPI.Aplication.Abstractions;
using TPI.Domain.Entities;

namespace TPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public ActionResult <Email> GetAll() 
        {
            return Ok(_emailService.GetAll());

        }


    }
}
