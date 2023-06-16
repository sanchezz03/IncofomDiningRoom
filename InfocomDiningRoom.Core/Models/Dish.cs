﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfocomDinnerRoom.Core.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TypeName { get; set; }
        public decimal CurrentPrice { get; set; }
        public string? Description { get; set; }
    }
}
