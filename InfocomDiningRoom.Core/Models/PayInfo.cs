using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class PayInfo
    {
        public int Id { get; set; }
        public decimal TotalAccured { get; set; }
        public decimal TotalPaid { get; set; }
        public int WeekId { get; set; }
        public int PersonalInfoId { get; set; }
    }
}
