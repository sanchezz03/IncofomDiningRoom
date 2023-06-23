using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncofomDiningRoom.WebApi.Controllers.Request.UserRequest
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("getdata")]
        public async Task<IActionResult> GetData(int weekNumber)
        {
            var data = await _unitOfWork.UsersRequests.GetMenuAndTotalCost(weekNumber);

            return Ok(data);
        }

        [HttpPut]
        [Route("ChangeCount")]
        public async Task<IActionResult> ChangeCount(string dishName, int count)
        {
           await _unitOfWork.UsersRequests.UpdateOrderDetailsCount(dishName, count);

            return Ok();
        }
        [HttpGet]
        [Route("GetBalance")]
        public async Task<IActionResult> GetBalance(int weekNumber)
        {
            var balance = await _unitOfWork.UsersRequests.UpdateUserBalanceBasedOnOrderCost(weekNumber);

            return Ok(balance);
        }
    }
}
