using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Auth
{
    public class User
    {
        public string? userName { get; set; }
        public string? password { get; set; }

        public string? role { get; set; }
    }
}
