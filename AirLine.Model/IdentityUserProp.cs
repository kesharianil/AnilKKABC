using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class IdentityUserProp
    {
        //[Key]
        //public string Id { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50, ErrorMessage = "Email max length should be 50 characters")]
        public string EmailId { get; set; }
        [Required]
        public string PanNo { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(10, ErrorMessage = "Password Must be between 8 and 10 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(10, ErrorMessage = "Password Must be between 8 and 10 characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
