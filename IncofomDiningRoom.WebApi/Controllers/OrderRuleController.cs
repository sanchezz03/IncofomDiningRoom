using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderRuleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderRuleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.OrderRules.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(OrderRules orderRule)
        {
            var data = await _unitOfWork.OrderRules.AddAsync(orderRule);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.OrderRules.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(OrderRules orderRule)
        {
            var data = await _unitOfWork.OrderRules.UpdateAsync(orderRule);

            return Ok(data);
        }
    }
}
