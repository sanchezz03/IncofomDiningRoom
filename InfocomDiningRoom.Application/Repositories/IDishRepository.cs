using InfocomDiningRoom.Core.Models.Dish;
using InfocomDinnerRoom.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Application.Repositories
{
    public interface IDishRepository : IGenericRepository<Dish>
    {
        Task<IReadOnlyList<DishInfo>> GetFirstDishes();
        Task<IReadOnlyList<DishInfo>> GetSecondDishes();
        Task<IReadOnlyList<DishInfo>> GetSalad();
        Task<IReadOnlyList<DishInfo>> GetDrinks();
        Task<IReadOnlyList<DishInfo>> FindDishesByName(string dishName);
    }
}
