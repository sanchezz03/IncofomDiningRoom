using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncofomDiningRoom.WebApi.Controllers.Request.AdminRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public RequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getdata")]
        public async Task<IActionResult> GetData(int weekNumber)
        {
            var data = await _unitOfWork.Admins.GetMenuPaymentInfo(weekNumber);

            return Ok(data);
        }
    }
}
