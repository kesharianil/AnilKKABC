﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class LoginResponseMessage
    {
        public string success { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public bool IsApprove { get; set; }
    }
}
