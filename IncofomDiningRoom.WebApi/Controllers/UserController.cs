using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [Authorize(Roles = "Адміністратор")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Users.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(User user)
        {
            var data = await _unitOfWork.Users.AddAsync(user);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Users.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(User user)
        {
            var data = await _unitOfWork.Users.UpdateAsync(user);

            return Ok(data);
        }

        [HttpPost]
        [Route("AllowableCreditAndOrderDeadline")]
        public async Task<IActionResult> AllowableCreditAndOrderDeadline(int allowableCredit, string orderDeadline)
        {
            TimeSpan parsedOrderDeadline;

            if (!TimeSpan.TryParse(orderDeadline, out parsedOrderDeadline))
            {
                return BadRequest("Invalid orderDeadline format");
            }

            var data = await _unitOfWork.Users.AddInfoAboutOrdersRule(allowableCredit, parsedOrderDeadline);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetNotActiveUsers")]
        public async Task<IActionResult> ShowNotActiveUsers()
        {
            var data = await _unitOfWork.Users.ShowNotActiveUsers();

            return Ok(data);
        }

        [HttpGet]
        [Route("GetUsersByWord")]
        public async Task<IActionResult> FindUsersByWord(string word)
        {
            var data = await _unitOfWork.Users.SearchUsersByWord(word);

            return Ok(data);
        }
    }
}
