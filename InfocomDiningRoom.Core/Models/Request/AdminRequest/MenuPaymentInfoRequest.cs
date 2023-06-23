using InfocomDiningRoom.Core.Models.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Request.AdminRequest
{
    public class MenuPaymentInfoRequest
    {
        public List<MenuInfoRequest> MenuInfoRequestList { get; set; }
        //public int SumOfAllCostOrders { get; set; }
    }
}
