using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
        public bool? isMailSend { get; set; }
    }
}
