using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class TokenProperty
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
        public IList<string>? roles { get; set; }
        public bool IsApprove { get; set; }
    }
}
