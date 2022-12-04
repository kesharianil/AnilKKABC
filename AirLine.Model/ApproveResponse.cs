using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class ApproveResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public bool IsApprove { get; set; }
        public string Name { get; set; }
    }
}
