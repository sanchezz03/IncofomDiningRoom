using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDiningRoom.Core.Models.Auth
{
    public class UserDto
    {
        public required string Username { get; set; }
        public string Password { get; set; }
    }
}
