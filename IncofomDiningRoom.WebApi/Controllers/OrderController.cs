using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Orders.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Order order)
        {
            var data = await _unitOfWork.Orders.AddAsync(order);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Orders.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Order order)
        {
            var data = await _unitOfWork.Orders.UpdateAsync(order);

            return Ok(data);
        }
    }
}
