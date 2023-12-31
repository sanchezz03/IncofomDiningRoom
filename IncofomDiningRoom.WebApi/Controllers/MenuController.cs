﻿using InfocomDiningRoom.Core.Models.Menu;
using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [Authorize(Roles = "Адміністратор, Користувач")]
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public MenuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [Authorize(Roles = "Адміністратор")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Menus.GetAllAsync();

            return Ok(data);
        }
        [Authorize(Roles = "Адміністратор")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Menu menu)
        {
            var data = await _unitOfWork.Menus.AddAsync(menu);

            return Ok(data);
        }
        [Authorize(Roles = "Адміністратор")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Menus.DeleteAsync(id);

            return Ok(data);
        }
        [Authorize(Roles = "Адміністратор")]
        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Menu menu)
        {
            var data = await _unitOfWork.Menus.UpdateAsync(menu);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetMenu")]
        public async Task<IActionResult> GetMenu(int weekNumber)
        {
            var data = await _unitOfWork.Menus.GetMenu(weekNumber);

            return Ok(data);
        }
        [Authorize(Roles = "Адміністратор")]
        [HttpPut]
        [Route("UpdateMenu")]
        public async Task<IActionResult> UpdateMenu(List<MenuInfo> menu)
        {
            await _unitOfWork.Menus.UpdateMenuInfo(menu);

            return Ok();
        }
    }
}
