using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Menu
{
    public class MenuInfo
    {
        public string WeekDay { get; set; }
        public List<MenuItemInfo> Dishes { get; set; }
        public decimal Cost { get; set; }
    }
}
