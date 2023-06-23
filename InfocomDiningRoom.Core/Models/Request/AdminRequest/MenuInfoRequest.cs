using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Request.AdminRequest
{
    public class MenuInfoRequest
    {
        public string FIO { get; set; }
        public Dictionary<string, Dictionary<string, int>> Weeks { get; set; }
    }
}
