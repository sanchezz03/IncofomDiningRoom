using InfocomDiningRoom.Core.Models.Menu;
using InfocomDiningRoom.Core.Models.Payment;
using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PayInfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PayInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.PayInfos.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(PayInfo payInfo)
        {
            var data = await _unitOfWork.PayInfos.AddAsync(payInfo);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.PayInfos.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(PayInfo payInfo)
        {
            var data = await _unitOfWork.PayInfos.UpdateAsync(payInfo);

            return Ok(data);
        }
        [HttpGet]
        [Route("PayInfo")]
        public async Task<IActionResult> GetPayInfo(int weekNumber)
        {
            var data = await _unitOfWork.PayInfos.GetMenuInfo(weekNumber);

            return Ok(data);
        }
        [HttpPut]
        [Route("UpdateBalance")]
        public async Task<IActionResult> UpdateBalance(MenuPaymentInfo menuInfos, decimal balance)
        {
            var data = await _unitOfWork.PayInfos.UpdateBalance(menuInfos, balance);

            return Ok(data);
        }
    }
}
