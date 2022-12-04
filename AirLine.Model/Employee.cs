using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Address { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        [StringLength(10)]
        public string PhoneNo { get; set; }
    }

    public class EmployeeListProp
    {
        public Employee employee { get; set; }
        public EmpolyeeOrganization empolyeeOrganizations { get; set; }
    }
}
