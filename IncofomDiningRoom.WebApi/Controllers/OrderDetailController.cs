using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [Authorize(Roles = "Адміністратор")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.OrderDetails.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(OrderDetails orderDetail)
        {
            var data = await _unitOfWork.OrderDetails.AddAsync(orderDetail);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.OrderDetails.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(OrderDetails orderDetail)
        {
            var data = await _unitOfWork.OrderDetails.UpdateAsync(orderDetail);

            return Ok(data);
        }
    }
}
