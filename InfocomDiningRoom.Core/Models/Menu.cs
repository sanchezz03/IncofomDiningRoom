using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class Menu
    {
        public int Id { get; set; }
        public string? WeekDay { get; set; }
        public int DishId { get; set; }
        public int WeekId { get; set; }
        public decimal Cost { get; set; }
    }
}
