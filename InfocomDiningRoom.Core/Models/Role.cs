using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsAdmin { get; set; }
        public string? StartView { get; set; }
    }
}
