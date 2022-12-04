using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class CreateAirlineModel
    {
        [Key]
        public long AirlineId { get; set; } = 0;
        [Required(ErrorMessage = "Airline Name is required")]

        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public string AirlineName { get; set; }
        [Required(ErrorMessage = "From City is required")]
        [StringLength(30)]
        public string FromCity { get; set; }
        [Required(ErrorMessage = "To City is required")]
        [StringLength(30)]
        public string ToCity { get; set; }

        [Required(ErrorMessage = "Fare is required")]
        //[StringLength(30)]
        public int Fare { get; set; }
    }
}
