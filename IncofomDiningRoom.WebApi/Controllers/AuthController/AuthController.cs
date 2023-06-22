using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncofomDiningRoom.WebApi.Controllers.AuthController
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var data = await _unitOfWork.Auths.Login(userName, password);

            if(data)
               return Ok();
            else
                return BadRequest();
        }
    }
}
