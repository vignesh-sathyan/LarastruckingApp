using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LarastruckingApp.Entities.Driver
{
    public class LeaveDto
    {
        public int LeaveId{ get; set; }
        public int DriverId { get; set; }

        public DateTime TakenFrom { get; set; }
        public DateTime TakenTo { get; set; }
        public string Reason { get; set; }
        public string LeaveStatus { get; set; }
        public int AppliedBy { get; set; }
        public DateTime AppliedOn { get; set; }

    }
}
