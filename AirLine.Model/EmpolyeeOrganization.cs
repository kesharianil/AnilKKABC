using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class EmpolyeeOrganization
    {

        [Key]
        public int Id { get; set; }
      
        [Column(TypeName = "varchar(50)")]
        public string OrganizationName { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }

        [Display(Name = "FEmployeeId")]
        public int FEmployeeId { get; set; }

        [ForeignKey("FEmployeeId")]
        public Employee Employee { get; set; }
    }
}
