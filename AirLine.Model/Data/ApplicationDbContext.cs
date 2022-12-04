using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CreateAirlineModel> Tbl_AirLines { get; set; }
        public DbSet<EmpolyeeOrganization> Tbl_EmpOrganization { get; set; }
        public DbSet<Employee> Tbl_Employee { get; set; }
       
    }
}
