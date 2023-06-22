using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Balance
{
    public class WeekData
    {
        public int WeekNumber { get; set; }
        public decimal TotalAccured { get; set; }
        public decimal TotalPaid { get; set; }
    }
}
