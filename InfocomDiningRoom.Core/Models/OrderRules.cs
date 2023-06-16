using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class OrderRules
    {
        public int Id { get; set; }
        public int AllowableCredit { get; set; }
        public TimeSpan OrderDeadline { get; set; }
    }
}
