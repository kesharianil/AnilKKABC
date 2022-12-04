using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email Id is required")]
        public string? EmailId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
