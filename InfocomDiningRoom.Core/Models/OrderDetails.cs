using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int DishId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
    }
}
