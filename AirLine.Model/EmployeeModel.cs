using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirLine.Model
{
    public class EmployeeModel
    {
        [JsonProperty("employeeId")]
        public long EmployeeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phoneNo")]
        public string PhoneNo { get; set; }
    }
}
