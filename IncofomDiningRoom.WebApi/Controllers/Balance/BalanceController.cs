using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncofomDiningRoom.WebApi.Controllers.Balance
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public BalanceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Route("GetBalance")]
        public async Task<IActionResult> GetBalance(int startWeekNumber, int endWeekNumber)
        {
            var data = await _unitOfWork.Balances.GetBalance(startWeekNumber,endWeekNumber);

            return Ok(data);
        }
    }
}
