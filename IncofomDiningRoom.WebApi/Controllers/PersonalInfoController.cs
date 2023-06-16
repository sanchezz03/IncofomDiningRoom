using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonalInfoController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonalInfoController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.PersonalInfos.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(PersonalInfo personalInfo)
        {
            var data = await _unitOfWork.PersonalInfos.AddAsync(personalInfo);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.PersonalInfos.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(PersonalInfo personalInfo)
        {
            var data = await _unitOfWork.PersonalInfos.UpdateAsync(personalInfo);

            return Ok(data);
        }
    }
}
