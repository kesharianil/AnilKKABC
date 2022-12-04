using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class ApproveModel
    {
        [Required]
        public string EmailId { get; set; }
    }
}
