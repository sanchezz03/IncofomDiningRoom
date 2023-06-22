using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Dish
{
    public class DishInfo
    {
        public int Id { get; set; }
        public decimal CurrentPrice { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
