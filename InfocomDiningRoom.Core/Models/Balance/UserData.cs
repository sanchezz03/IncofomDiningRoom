using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Balance
{
    public class UserData
    {
        public string? FullName { get; set; }
        public List<WeekData>? WeeksData { get; set; }
        public decimal SumAccured { get; set; }
        public decimal SumTotalPaid { get; set; }
        public decimal Balance { get; set; }
    }
}
