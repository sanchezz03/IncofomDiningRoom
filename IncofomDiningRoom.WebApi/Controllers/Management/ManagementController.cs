using InfocomDiningRoom.Core.Models.Management;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IncofomDiningRoom.WebApi.Controllers.Management
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> Users()
        {
            var data = await _unitOfWork.Managements.Users();

            return Ok(data);
        }

        [HttpPut]
        [Route("EditUsers")]
        public async Task<IActionResult> EditUsers(int userId, string newUserName, string newEmail, int newRoleId)
        {
            var data = await _unitOfWork.Managements.EditUser(userId, newUserName, newEmail, newRoleId);

            return Ok(data);
        }


        [HttpGet]
        [Route("Roles")]
        public async Task<IActionResult> Roles()
        {
            var data = await _unitOfWork.Managements.Roles();

            return Ok(data);
        }

        [HttpPut]
        [Route("EditRole")]
        public async Task<IActionResult> EditRole(RoleInfo role)
        {
            var data = await _unitOfWork.Managements.EditRole(role);

            return Ok(data);
        }
    }
}
