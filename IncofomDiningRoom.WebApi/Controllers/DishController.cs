﻿using InfocomDinnerRoom.Core.Models;
using InfocomDinnerRoom.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfocomDinnerRoom.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DishController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DishController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.Dishes.GetAllAsync();

            return Ok(data);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(Dish dish)
        {
            var data = await _unitOfWork.Dishes.AddAsync(dish);

            return Ok(data);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _unitOfWork.Dishes.DeleteAsync(id);

            return Ok(data);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Dish dish)
        {
            var data = await _unitOfWork.Dishes.UpdateAsync(dish);

            return Ok(data);
        }

        [HttpGet]
        [Route("GetFirstDishes")]
        public async Task<IActionResult> GetFirstDishes()
        {
            var data = await _unitOfWork.Dishes.GetFirstDishes();

            return Ok(data);
        }
        [HttpGet]
        [Route("GetSecondDishes")]
        public async Task<IActionResult> GetSecondDishes()
        {
            var data = await _unitOfWork.Dishes.GetSecondDishes();

            return Ok(data);
        }
        [HttpGet]
        [Route("GetSalad")]
        public async Task<IActionResult> GetSalad()
        {
            var data = await _unitOfWork.Dishes.GetSalad();

            return Ok(data);
        }
        [HttpGet]
        [Route("GetDrinks")]
        public async Task<IActionResult> GetDrinks()
        {
            var data = await _unitOfWork.Dishes.GetDrinks();

            return Ok(data);
        }
    }
}
