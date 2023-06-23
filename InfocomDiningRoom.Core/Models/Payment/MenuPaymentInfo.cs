using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Payment
{
    public class MenuPaymentInfo
    {
        public string FIO { get; set; }
        public Dictionary<string, Dictionary<string, decimal>> Weeks { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Balance { get; set; }
    }
}
