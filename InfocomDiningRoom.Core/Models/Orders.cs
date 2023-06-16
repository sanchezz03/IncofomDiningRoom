using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public int UserId { get; set; }
        public decimal Cost { get; set; }
        public int OrderRulesId { get; set; }
    }
}
