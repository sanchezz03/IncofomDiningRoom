using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Payment
{
    public class MenuDay
    {
        public MenuRow FirstDish { get; set; }
        public MenuRow SecondDish { get; set; }
        public MenuRow Salad { get; set; }
        public MenuRow Drinks { get; set; }
    }
}
